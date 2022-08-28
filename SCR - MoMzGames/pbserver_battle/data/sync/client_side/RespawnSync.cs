using Battle.config;
using Battle.data.models;
using Battle.data.xml;
using Battle.network;
using Battle.network.packets;
using System;
using System.Collections.Generic;

namespace Battle.data.sync.client_side
{
    public static class RespawnSync
    {
        public static void Load(ReceivePacket p)
        {
            uint UniqueRoomId = p.readUD();
            int gen2 = p.readD();
            long roomTick = p.readQ();
            int syncType = p.readC();
            int round = p.readC();
            int slotId = p.readC();
            int spawnNumber = p.readC();
            byte accountId = p.readC();
            int type = 0, number = 0, HPBonus = 0;
            bool C4Speed = false;
            if (syncType == 0 || syncType == 2)
            {
                type = p.readC();
                number = p.readH();
                HPBonus = p.readC();
                C4Speed = p.readC() == 1;
                if (28 < p.getBuffer().Length)
                    Logger.warning("[ALTO]: " + BitConverter.ToString(p.getBuffer()));
            }
            else
                if (23 < p.getBuffer().Length)
                    Logger.warning("[ALTO]: " + BitConverter.ToString(p.getBuffer()));
            Room room = RoomsManager.getRoom(UniqueRoomId);
            if (room == null)
                return;

            room.ResyncTick(roomTick, gen2); //Colocar dentro do IF
            Player player = room.getPlayer(slotId, true);
            if (player != null && player._playerIdByUser != accountId)
                Logger.warning("Invalid User Ids: [By user: " + player._playerIdByUser + "/ Server: " + accountId + "]");
            if (player != null && player._playerIdByUser == accountId)
            {
                player._playerIdByServer = accountId;
                player._respawnByServer = spawnNumber;
                player.Integrity = false;
                if (round > room._serverRound)
                    room._serverRound = round;
                if (syncType == 0 || syncType == 2) //0 = Entrando na partida (normalmente) | 1 = Entrando na partida de espectador (Destru/Supressão) | 2 = Entrando na partida (normalmente/não é o primeiro respawn do jogador)
                {
                    player._respawnByLogic++;
                    player.isDead = false;
                    player._plantDuration = Config.plantDuration;
                    player._defuseDuration = Config.defuseDuration;
                    if (C4Speed)
                    {
                        player._plantDuration -= AllUtils.percentage(Config.plantDuration, 50);
                        player._defuseDuration -= AllUtils.percentage(Config.defuseDuration, 25);
                    }
                    if (!room._isBotMode)
                    {
                        if (room._sourceToMap == -1)
                            room.RoundResetRoomF1(round);
                        else
                            room.RoundResetRoomS1(round);
                    }
                    if (type == 255)
                        player.Immortal = true;
                    else
                    {
                        player.Immortal = false;
                        int CharaHP = CharaXML.getLifeById(number, type);
                        CharaHP += AllUtils.percentage(CharaHP, HPBonus);
                        player._maxLife = CharaHP;
                        player.ResetLife();
                    }
                }
                if (room._isBotMode || syncType == 2 || !room.ObjectsIsValid())
                    return;

                List<ObjectHitInfo> syncList = new List<ObjectHitInfo>();
                for (int i = 0; i < room._objects.Length; i++)
                {
                    ObjectInfo rObj = room._objects[i];
                    ObjModel mObj = rObj._model;
                    if (mObj != null && (syncType != 2 && mObj.isDestroyable && rObj._life != mObj._life || mObj._needSync))
                        syncList.Add(new ObjectHitInfo(3)
                        {
                            objSyncId = mObj._needSync ? 1 : 0,
                            _animId1 = mObj._anim1,
                            _animId2 = rObj._anim != null ? rObj._anim._id : 255,
                            _destroyState = rObj.DestroyState,
                            objId = mObj._id,
                            objLife = rObj._life,
                            _specialUse = AllUtils.GetDuration(rObj._useDate)
                        });
                }
                for (int i = 0; i < room._players.Length; i++)
                {
                    Player pR = room._players[i];
                    if (pR._slot != slotId && pR.AccountIdIsValid() && !pR.Immortal && pR._date != new DateTime() && (pR._maxLife != pR._life || pR.isDead))
                        syncList.Add(new ObjectHitInfo(4)
                        {
                            objId = pR._slot,
                            objLife = pR._life
                        });
                }
                if (syncList.Count > 0)
                {
                    byte[] packet = Packet4Creator.getCode4(Packet4Creator.getCode4SyncData(syncList), room._startTime, round, 255);
                    BattleManager.Send(packet, player._client);
                }
                syncList = null;
            }
        }
    }
}