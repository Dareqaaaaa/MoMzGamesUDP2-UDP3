/*
 * Arquivo: Room.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 05/08/2017
 * Sinta inveja, não nos atinge
 */

using Battle.config;
using Battle.data.xml;
using Battle.network;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Net;

namespace Battle.data.models
{
    public class Room
    {
        public Player[] _players = new Player[16];
        public ObjectInfo[] _objects = new ObjectInfo[200];
        public DateTime LastObjsSync, LastPlayersSync;
        public uint UniqueRoomId;
        public int _objsSyncRound, _serverRound, _genId2, _sourceToMap = -1, _serverId, _mapId, stageType, _roomId, _channelId, LastRound, _dropCounter,
            _bar1 = 6000,
            _bar2 = 6000,
            _default1 = 6000,
            _default2 = 6000;
        public GameServerModel gs;
        public MapModel Map;
        private object _lock = new object(),
            _lock2 = new object();
        public bool _isBotMode, _hasC4;
        public long LastStartTick;
        public Half3 BombPosition;
        public DateTime _startTime;
        public Room(int serverId)
        {
            gs = ServersXML.getServer(serverId);
            if (gs == null)
                return;
            _serverId = serverId;
            for (int i = 0; i < 16; ++i)
                _players[i] = new Player(i);
            for (int i = 0; i < 200; ++i)
                _objects[i] = new ObjectInfo(i);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="type">1 = Obj|2 = Players</param>
        public void SyncInfo(List<ObjectHitInfo> objs, int type)
        {
            lock (_lock2)
            {
                if (_isBotMode || !ObjectsIsValid())
                    return;
                DateTime now = DateTime.Now;
                double value = (now - LastObjsSync).TotalSeconds;
                double value2 = (now - LastPlayersSync).TotalSeconds;
                if (value >= 2.5 && (type & 1) == 1)
                {
                    LastObjsSync = now;
                    for (int i = 0; i < _objects.Length; i++)
                    {
                        ObjectInfo rObj = _objects[i];
                        ObjModel mObj = rObj._model;
                        if (mObj != null && (mObj.isDestroyable && rObj._life != mObj._life || mObj._needSync))
                        {
                            float SyncingTime = AllUtils.GetDuration(rObj._useDate);
                            AnimModel anim = rObj._anim;
                            if (anim != null && anim._duration > 0 && SyncingTime >= anim._duration)
                                mObj.GetAnim(anim._nextAnim, SyncingTime, anim._duration, rObj);
                            objs.Add(new ObjectHitInfo(mObj._updateId)
                            {
                                objSyncId = mObj._needSync ? 1 : 0,
                                _animId1 = mObj._anim1,
                                _animId2 = rObj._anim != null ? rObj._anim._id : 255,
                                _destroyState = rObj.DestroyState,
                                objId = mObj._id,
                                objLife = rObj._life,
                                _specialUse = SyncingTime
                            });
                        }
                    }
                }
                if (value2 >= 6.5 && (type & 2) == 2)
                {
                    LastPlayersSync = now;
                    for (int i = 0; i < _players.Length; i++)
                    {
                        Player pl = _players[i];
                        if (!pl.Immortal && (pl._maxLife != pl._life || pl.isDead))
                            objs.Add(new ObjectHitInfo(4)
                            {
                                objId = pl._slot,
                                objLife = pl._life
                            });
                    }
                }
            }
        }
        public bool RoundIsValid(int number)
        {
            //if (_lastRound != number && (_lastRound != number + 1 && _lastRound + 1 != number))
            //    Logger.warning("[" + _lastRound + "/" + number + "] Invalid round on - Room: " + _roomId + "; Ch: " + _channelId + "; Type: " + _roomType);
            return LastRound == number || LastRound + 1 == number;// || _lastRound == number + 1;
        }
        public bool ObjectsIsValid()
        {
            return _serverRound == _objsSyncRound;
        }
        /// <summary>
        /// Checa se a data de ínicio da partida é válida. Caso seja válida, reseta as informações da sala, como objetos, informações de drop..
        /// </summary>
        /// <param name="newvalue">Nova data</param>
        /// <param name="gen2">Informações da sala (Mapa/Tipo)</param>
        public void ResyncTick(long startTick, int gen2)
        {
            if (startTick > LastStartTick)
            {
                _startTime = new DateTime(startTick);
                if (LastStartTick > 0)
                    ResetRoomInfo(gen2);
                LastStartTick = startTick;
                if (Config.isTestMode)
                    Logger.warning("[New tick is defined]");
            }
        }
        public void ResetRoomInfo(int gen2)
        {
            for (int i = 0; i < 200; ++i)
                _objects[i] = new ObjectInfo(i);
            _mapId = RoomsManager.getGenV(gen2, 1);
            stageType = RoomsManager.getGenV(gen2, 2);
            _sourceToMap = -1;
            Map = null;
            LastRound = 0;
            _dropCounter = 0;
            _isBotMode = false;
            _hasC4 = false;
            _serverRound = 0;
            _objsSyncRound = 0;
            LastObjsSync = new DateTime();
            LastPlayersSync = new DateTime();
            BombPosition = new Half3();
            if (Config.isTestMode)
                Logger.warning("A room has been reseted by server.");
        }
        /// <summary>
        /// Reseta as informações da sala, caso a rodada seja válida.
        /// </summary>
        /// <param name="round">Rodada</param>
        /// <returns></returns>
        public bool RoundResetRoomF1(int round)
        {
            lock (_lock)
            {
                if (LastRound != round)
                {
                    if (Config.isTestMode)
                        Logger.warning("Reseting room. [Last: " + LastRound + "; New: " + round + "]");
                    DateTime now = DateTime.Now;
                    LastRound = round;
                    _hasC4 = false;
                    BombPosition = new Half3();
                    _dropCounter = 0;
                    _objsSyncRound = 0;
                    _sourceToMap = _mapId;
                    if (!_isBotMode)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            Player player = _players[i];
                            player._life = player._maxLife;
                        }
                        LastPlayersSync = now;
                        Map = MappingXML.getMapById(_mapId);
                        List<ObjModel> objsm = Map != null ? Map._objects : null;
                        if (objsm != null)
                        {
                            for (int i = 0; i < objsm.Count; i++)
                            {
                                ObjModel ob = objsm[i];
                                ObjectInfo obj = _objects[ob._id];
                                obj._life = ob._life;
                                if (!ob._noInstaSync)
                                    ob.GetARandomAnim(this, obj);
                                else
                                {
                                    obj._anim = new AnimModel { _nextAnim = 1 };
                                    obj._useDate = now;
                                }
                                obj._model = ob;
                                obj.DestroyState = 0;
                                MappingXML.SetObjectives(ob, this);
                            }
                        }
                        LastObjsSync = now;
                        _objsSyncRound = round;
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Reseta as informações da sala, caso a rodada seja válida.
        /// </summary>
        /// <param name="round">Rodada</param>
        /// <returns></returns>
        public bool RoundResetRoomS1(int round)
        {
            lock (_lock)
            {
                if (LastRound != round)
                {
                    if (Config.isTestMode)
                        Logger.warning("Reseting room. [Last: " + LastRound + "; New: " + round + "]");
                    LastRound = round;
                    _hasC4 = false;
                    _dropCounter = 0;
                    BombPosition = new Half3();
                    if (!_isBotMode)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            Player player = _players[i];
                            player._life = player._maxLife;
                        }
                        DateTime now = DateTime.Now;
                        LastPlayersSync = now;
                        for (int i = 0; i < _objects.Length; i++)
                        {
                            ObjectInfo obj = _objects[i];
                            ObjModel ob = obj._model;
                            if (ob != null)
                            {
                                obj._life = ob._life;
                                if (!ob._noInstaSync)
                                    ob.GetARandomAnim(this, obj);
                                else
                                {
                                    obj._anim = new AnimModel { _nextAnim = 1 };
                                    obj._useDate = now;
                                }
                                obj.DestroyState = 0;
                            }
                        }
                        LastObjsSync = now;
                        _objsSyncRound = round;
                        if ((stageType == 3 || stageType == 5))
                        {
                            _bar1 = _default1;
                            _bar2 = _default2;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        public Player AddPlayer(IPEndPoint client, PacketModel packet, string udp)
        {
            if (Config.udpVersion != udp)
                return null;
            try
            {
                Player pl = _players[packet._slot];
                if (!pl.CompareIP(client))
                {
                    pl._client = client;
                    pl._date = packet._receiveDate;
                    pl._playerIdByUser = packet._accountId;
                    return pl;
                }
            }
            catch (Exception ex) 
            { 
                Logger.warning(ex.ToString()); 
            }
            return null;
        }
        public bool getPlayer(int slot, out Player p)
        {
            try
            {
                p = _players[slot];
            }
            catch
            {
                p = null;
            }
            return p != null;
        }
        public Player getPlayer(int slot, bool isActive)
        {
            Player p;
            try
            {
                p = _players[slot];
            }
            catch
            {
                p = null;
            }
            return p != null && (!isActive || p._client != null) ? p : null;
        }
        public Player getPlayer(int slot, IPEndPoint client)
        {
            Player p;
            try
            {
                p = _players[slot];
            }
            catch
            {
                p = null;
            }
            return p != null && p.CompareIP(client) ? p : null;
        }
        public ObjectInfo getObject(int id)
        {
            ObjectInfo obj;
            try
            {
                obj = _objects[id];
            }
            catch
            {
                obj = null;
            }
            return obj;
        }
        public bool RemovePlayer(int slot, IPEndPoint ip)
        {
            try
            {
                Player p = _players[slot];
                if (p.CompareIP(ip))
                {
                    p.ResetAllInfos();
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
        public int getPlayersCount()
        {
            int players = 0;
            for (int i = 0; i < 16; i++)
                if (_players[i]._client != null)
                    players++;
            return players;
        }
    }
}