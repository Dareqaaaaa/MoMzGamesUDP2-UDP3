using Battle.config;
using Battle.data;
using Battle.data.enums;
using Battle.data.enums.bomb;
using Battle.data.enums.weapon;
using Battle.data.models;
using Battle.data.sync;
using Battle.data.xml;
using Battle.network.actions.damage;
using Battle.network.actions.others;
using Battle.network.actions.user;
using Battle.network.packets;
using SharpDX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Battle.network
{
    public class BattleHandler
    {
        private UdpClient _client;
        private IPEndPoint remoteEP;
        public BattleHandler(UdpClient client, byte[] buff, IPEndPoint remote, DateTime date)
        {
            _client = client;
            remoteEP = remote;
            BeginReceive(buff, date);
        }
        public void BeginReceive(byte[] buffer, DateTime date)
        {
            PacketModel packet = new PacketModel();
            packet._data = buffer;
            packet._receiveDate = date;
            ReceivePacket rec = new ReceivePacket(packet._data);
            packet._opcode = rec.readC();
            packet._slot = rec.readC();
            packet._time = rec.readT();
            packet._round = rec.readC();
            packet._length = rec.readUH();
            packet._respawnNumber = rec.readC(); //Respawn atual
            packet._roundNumber = rec.readC(); //round/passagens no portal
            packet._accountId = rec.readC();
            packet._unkInfo2 = rec.readC();
            if (packet._length > packet._data.Length)
            {
                Logger.warning("Pacote com tamanho inválido cancelado. [Length: " + packet._length + "; DataLength: " + packet._data.Length + "]");
                return;
            }
            getDecryptedData(packet);

            if (Config.isTestMode && packet._unkInfo2 > 0)
            {
                Logger.warning("[N] Unk: " + packet._unkInfo2 + " [" + BitConverter.ToString(packet._data) + "]");
                Logger.warning("[D] Unk: " + packet._unkInfo2 + " [" + BitConverter.ToString(packet._withEndData) + "]");
            }
            //Logger.warning("[" + packet._opcode + "]: " + BitConverter.ToString(dataNoEnd));
            if (Config.enableLog)
            {
                //byte[] bufferNoEnc2 = AllUtils.decrypt(buffer, packet._length % 6 + 1);
                //if (packet._opcode == 131 || packet._opcode == 132 || packet._opcode == 3 || packet._opcode == 4)
                //    Logger.warning("[" + DateTime.Now.ToString("yyyyMMdd:HH:mm:ss") + "] op: " + packet._opcode + " ; " + BitConverter.ToString(bufferNoEnc2));
                if (packet._opcode == 3 || packet._opcode == 4 || packet._opcode == 131 || packet._opcode == 132)
                    LoggerTestMode(packet._noEndData, packet);
            }
            ReadPacket(packet);
        }
        /// <summary>
        /// Gera pacotes desencriptados.
        /// </summary>
        /// <param name="packet">Pacote recebido</param>
        public void getDecryptedData(PacketModel packet)
        {
            byte[] withEnd, noEnd;
            if (packet._data.Length >= packet._length)
            {
                byte[] result = new byte[packet._length - 13];
                Array.Copy(packet._data, 13, result, 0, result.Length);

                withEnd = AllUtils.decrypt(result, packet._length % 6 + 1);
                noEnd = new byte[withEnd.Length - 9];
                Array.Copy(withEnd, noEnd, noEnd.Length);
                packet._withEndData = withEnd;
                packet._noEndData = noEnd;
            }
            else
                throw new Exception("Tamanho do pacote inválido.");
        }
        public void ReadPacket(PacketModel packet)
        {
            byte[] withEndData = packet._withEndData;
            byte[] noEndData = packet._noEndData;

            ReceivePacket rec = new ReceivePacket(withEndData);
            int BasicBufferLength = noEndData.Length, gen2 = 0, dedicationSlot = 0;
            uint UniqueRoomId = 0;
            Room room = null;
            try
            {
                switch (packet._opcode)
                {
                    case 131: //BOT Sync INFO (By Player)
                    case 132: //BOT Sync INFO (By Host)
                        rec.Advance(BasicBufferLength);
                        UniqueRoomId = rec.readUD();
                        dedicationSlot = rec.readC();
                        gen2 = rec.readD();
                        room = RoomsManager.getRoom(UniqueRoomId);
                        if (room != null)
                        {
                            Player player = room.getPlayer(packet._slot, remoteEP);
                            if (player != null && player.AccountIdIsValid(packet._accountId))
                            {
                                room._isBotMode = true;
                                Player p2 = dedicationSlot != 255 ? room.getPlayer(dedicationSlot, false) : null;
                                byte[] code;
                                if (p2 != null)
                                    code = Packet132Creator.getCode132(noEndData, p2._date, packet._round, dedicationSlot);
                                else
                                    code = Packet132Creator.getCode132(noEndData, player._date, packet._round, packet._slot);
                                for (int num = 0; num < 16; num++)
                                {
                                    Player playerR = room._players[num];
                                    if (playerR._client != null && playerR.AccountIdIsValid() && num != packet._slot)
                                        Send(code, playerR._client);
                                }
                            }
                        }
                        break;
                    case 97:
                        UniqueRoomId = rec.readUD();
                        dedicationSlot = rec.readC();
                        gen2 = rec.readD();
                        room = RoomsManager.getRoom(UniqueRoomId);
                        if (room != null)
                        {
                            Player player = room.getPlayer(packet._slot, remoteEP);
                            if (player != null)
                            {
                                player.LastPing = packet._receiveDate;
                                Send(packet._data, remoteEP);
                            }
                        }
                        break;
                    case 4:
                    case 3: //recebe dos usuários
                        rec.Advance(BasicBufferLength);
                        UniqueRoomId = rec.readUD();
                        dedicationSlot = rec.readC(); //Slot dedicado (255=Servidor|Outro=Host da sala)
                        gen2 = rec.readD();
                        room = RoomsManager.getRoom(UniqueRoomId);
                        if (room != null)
                        {
                            Player player = room.getPlayer(packet._slot, remoteEP);
                            if (player != null && player.AccountIdIsValid(packet._accountId))
                            {
                                player._respawnByUser = packet._respawnNumber;
                                if (packet._opcode == 4)
                                    room._isBotMode = true;
                                if (room._startTime == new DateTime())
                                    return;
                                byte[] actions = WriteActionBytes(noEndData, room, AllUtils.GetDuration(player._date), packet);

                                bool useMyDate = packet._opcode == 4 && dedicationSlot == 255;

                                int valor = 0;
                                if (useMyDate)
                                    valor = packet._slot;
                                else if (packet._opcode == 3)
                                    valor = room._isBotMode ? packet._slot : 255;
                                else
                                    Logger.warning("[Dedication invalid] d: " + dedicationSlot + "; s: " + packet._slot + "; opc: " + packet._opcode);

                                byte[] code = Packet4Creator.getCode4(actions, useMyDate ? player._date : room._startTime, packet._round, valor);
                                bool V3 = packet._opcode == 3 && !room._isBotMode && dedicationSlot != 255;
                                for (int i = 0; i < 16; i++)
                                {
                                    bool V1 = i != packet._slot;
                                    Player playerR = room._players[i];
                                    if (playerR._client != null && playerR.AccountIdIsValid() &&
                                        (dedicationSlot == 255 && V1 ||
                                        packet._opcode == 3 && room._isBotMode && V1 || V3))
                                        Send(code, playerR._client);
                                }
                            }
                        }
                        break;
                    case 65:
                        string udpVersion = rec.readD() + "." + rec.readD();
                        UniqueRoomId = rec.readUD();
                        gen2 = rec.readD();
                        dedicationSlot = rec.readC();
                        room = RoomsManager.CreateOrGetRoom(UniqueRoomId, gen2);
                        if (room != null)
                        {
                            Player player = room.AddPlayer(remoteEP, packet, udpVersion);
                            if (player != null)
                            {
                                if (!player.Integrity)
                                    player.ResetBattleInfos();
                                byte[] code = Packet66Creator.getCode66();
                                Send(code, player._client);
                                if (Config.isTestMode)
                                    Logger.warning("Player connected. [" + player._client.Address + ":" + player._client.Port + "]");
                            }
                        }
                        break;

                    case 67:
                        byte[] unk = rec.readB(8);
                        UniqueRoomId = rec.readUD();
                        gen2 = rec.readD();
                        dedicationSlot = rec.readC();
                        room = RoomsManager.getRoom(UniqueRoomId);
                        //ReceivePacket specialBuffer2 = new ReceivePacket(packet._data);
                        //specialBuffer2.readB(9);
                        //byte[] someBytes2 = specialBuffer2.readB(4);
                        //Logger.warning("[" + DateTime.Now.ToString("yy/MM/dd HH:mm") + "] Received disconnect1: " + BitConverter.ToString(someBytes2));
                        //Logger.warning("[" + DateTime.Now.ToString("yy/MM/dd HH:mm") + "] Received disconnect2: " + BitConverter.ToString(unk));
                        if (room != null)
                        {
                            if (room.RemovePlayer(packet._slot, remoteEP))
                                if (Config.isTestMode)
                                    Logger.warning("Player disconnected. [" + remoteEP.Address + ":" + remoteEP.Port + "]");
                            if (room.getPlayersCount() == 0)
                                RoomsManager.RemoveRoom(room.UniqueRoomId);
                        }
                        break;
                    default:
                        Logger.warning("Pacote inválido: " + packet._opcode);
                        Logger.warning("[Decrypted2] Data: " + BitConverter.ToString(withEndData));
                        Logger.warning("[Encrypted2] Data: " + BitConverter.ToString(packet._data));
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
                Logger.warning("[Decrypted] Data: " + BitConverter.ToString(withEndData));
                Logger.warning("[Encrypted] Data: " + BitConverter.ToString(packet._data));
            }
        }
        public void LoggerTestMode(byte[] data, PacketModel packet)
        {
            ReceivePacket p = new ReceivePacket(data);
            if (packet._opcode == 131 || packet._opcode == 132)
                p.readT();
            //Logger.warning("P: " + BitConverter.ToString(data));
            for (int i = 0; i < 16; i++)
            {
                ActionModel ac = new ActionModel();
                try
                {
                    bool exception;
                    ac._type = (P2P_SUB_HEAD)p.readC(out exception);
                    if (exception) break;
                    ac._slot = p.readUH();
                    ac._lengthData = p.readUH();
                    if (ac._lengthData == 65535)
                        break;
                    if (ac._type == P2P_SUB_HEAD.GRENADE)
                        code1_GrenadeSync.ReadInfo(p, false, true);
                    else if (ac._type == P2P_SUB_HEAD.DROPEDWEAPON)
                        code2_WeaponSync.ReadInfo(p, false);
                    else if (ac._type == P2P_SUB_HEAD.OBJECT_STATIC)
                        code3_ObjectStatic.ReadInfo(p, false);
                    else if (ac._type == P2P_SUB_HEAD.OBJECT_ANIM)
                        code6_ObjectAnim.ReadInfo(p, false);
                    else if (ac._type == P2P_SUB_HEAD.STAGEINFO_OBJ_STATIC)
                        code9_StageInfoObjStatic.readSyncInfo(p, false);
                    else if (ac._type == P2P_SUB_HEAD.STAGEINFO_OBJ_ANIM)
                        code12_StageObjAnim.ReadInfo(p, true);
                    else if (ac._type == P2P_SUB_HEAD.CONTROLED_OBJECT)
                        code13_ControledObj.readSyncInfo(p, true);
                    else if (ac._type == P2P_SUB_HEAD.USER || ac._type == P2P_SUB_HEAD.STAGEINFO_CHARA)
                    {
                        ac._flags = (Events)p.readUD();
                        ac._data = p.readB(ac._lengthData - 9);
                        GetUserLogs(data, ac);
                    }
                    else
                        Logger.warning("[New user packet type '" + ac._type + "' or '" + (int)ac._type + "']: " + BitConverter.ToString(data));
                }
                catch (Exception ex)
                {
                    Logger.warning(ex.ToString());
                    Logger.warning("1: " + BitConverter.ToString(packet._data));
                    Logger.warning("2: " + BitConverter.ToString(data));
                    break;
                }
            }
        }
        private void GetUserLogs(byte[] data, ActionModel ac)
        {
            if (ac._data.Length == 0)
                return;
            ReceivePacket rec = new ReceivePacket(ac._data);
            uint contador = 0;
            if (ac._flags.HasFlag((Events)1))
            {
                contador++;
                a1_unk.ReadInfo(rec, false);
            }
            if (ac._flags.HasFlag((Events)2))
            {
                contador += 2;
                a2_unk.ReadInfo(ac, rec, false);
            }
            if (ac._flags.HasFlag(Events.PosRotation))
            {
                contador += 4;
                a4_PositionSync.ReadInfo(rec, false);
            }
            if (ac._flags.HasFlag((Events)8)) //Localização no mapa
            {
                contador += 8;
                a8_MoveSync.readSyncInfo(ac, rec, false);
            }
            if (ac._flags.HasFlag((Events)16))
            {
                contador += 16;
                a10_unk.readSyncInfo(ac, rec, false);
            }
            if (ac._flags.HasFlag(Events.RadioChat))
            {
                contador += 32;
                a20_RadioSync.readSyncInfo(ac, rec, false);
            }
            if (ac._flags.HasFlag((Events)64))
            {
                contador += 64;
                a40_WeaponSync.ReadInfo(ac, rec, false, true);
            }
            if (ac._flags.HasFlag((Events)128))
            {
                contador += 128;
                a80_WeaponRecoil.ReadInfo(ac, rec, false);
            }
            if (ac._flags.HasFlag(Events.LifeSync))
            {
                contador += 256;
                a100_LifeSync.ReadInfo(rec, false);
            }
            if (ac._flags.HasFlag(Events.Suicide))
            {
                contador += 512;
                a200_SuicideDamage.ReadInfo(rec, false, true);
            }
            if (ac._flags.HasFlag((Events)1024))
            {
                contador += 1024;
                a400_Mission.ReadInfo(ac, rec, false, 0, true);
            }
            if (ac._flags.HasFlag(Events.TakeWeapon))
            {
                contador += 2048;
                a800_WeaponAmmo.ReadInfo(ac, rec, false);
            }
            if (ac._flags.HasFlag(Events.DropWeapon))
            {
                contador += 4096;
                a1000_DropWeapon.ReadInfo(rec, true);
            }
            if (ac._flags.HasFlag(Events.FireSync))
            {
                contador += 8192;
                a2000_FireSync.ReadInfo(rec, false);
            }
            if (ac._flags.HasFlag(Events.AIDamage))
            {
                contador += 16384;
                a4000_BotHitData.ReadInfo(rec, false);
            }
            if (ac._flags.HasFlag(Events.NormalDamage))
            {
                contador += 32768;
                a8000_NormalHitData.ReadInfo(rec, false, true);
            }
            if (ac._flags.HasFlag(Events.BoomDamage))
            {
                contador += 65536;
                a10000_BoomHitData.ReadInfo(rec, false, true);
            }
            if (ac._flags.HasFlag(Events.Death))
            {
                contador += 262144;
                a40000_DeathData.ReadInfo(ac, rec, false);
            }
            if (ac._flags.HasFlag((Events)524288))
            {
                contador += 524288;
                a80000_SufferingDamage.ReadInfo(ac, rec, false);
            }
            if (ac._flags.HasFlag(Events.PassPortal))
            {
                contador += 1048576;
                a100000_PassPortal.ReadInfo(ac, rec, false);
            }
            if (contador != (uint)ac._flags)
                Logger.warning("[" + (uint)ac._flags + "|" + ((uint)ac._flags - contador) + "] BUF ANORMAL: " + BitConverter.ToString(data));
        }
        private void RemoveHit(IList list, int idx)
        {
            list.RemoveAt(idx);
        }
        public List<ObjectHitInfo> getNewActions(ActionModel ac, Room room, float time, out byte[] EventsData)
        {
            EventsData = new byte[0];
            if (room == null)
                return null;
            if (ac._data.Length == 0)
                return new List<ObjectHitInfo>();
            byte[] data = ac._data;
            List<ObjectHitInfo> objs = new List<ObjectHitInfo>();
            ReceivePacket rec = new ReceivePacket(data);
            using (SendPacket sp = new SendPacket())
            {
                int contador = 0;
                Player pl = room.getPlayer(ac._slot, true);
                if (ac._flags.HasFlag(Events.ActionState))
                {
                    contador++;
                    a1_unk.Struct info = a1_unk.ReadInfo(rec, false);
                    a1_unk.writeInfo(sp, info);
                    /*uint all = BitConverter.ToUInt32(new byte[] { info._unkV, info._unkV2, info._unkV3, info._unkV4 }, 0);
                    //(_BYTE*)(a2 + 11) & 3 | 4 * *(_DWORD*)(a2 + 56) | (*(_BYTE*)(a2 + 7) << 10) | (*(_BYTE*)(a2 + 6) << 14) | (*(_BYTE*)(a2 + 10) << 15) | (*(_BYTE*)(a2 + 9) << 16) | (*(_BYTE*)(a2 + 8) << 20);
                    uint code = (all & 3);
                    uint code1 = (all >> 2) & 1; //all >> 2 & 1
                    uint code2 = (all >> 10) & 0xF; //all >> 10 & 0xF
                    uint code3 = (all >> 14) & 1; //all >> 14 & 1
                    uint code4 = (all >> 15) & 1; //all >> 15 & 1
                    uint code5 = (all >> 16) & 0xF; //all >> 16 & 0xF
                    uint code6 = (all >> 20) & 0xFF; //all >> 20 & 0xFF
                    *///Logger.warning("1: " + code + "; " + code2 + "; " + code3 + "; " + code4 + "; " + code5 + "; " + code6);
                }
                if (ac._flags.HasFlag(Events.Animation))
                {
                    contador += 2;
                    a2_unk.writeInfo(sp, ac, rec, false);
                }
                if (ac._flags.HasFlag(Events.PosRotation))
                {
                    contador += 4;
                    a4_PositionSync.Struct info = a4_PositionSync.ReadInfo(rec, false);
                    a4_PositionSync.writeInfo(sp, info);
                    //Half3 half = new Half3(info._rotationX, info._rotationY, info._rotationZ);
                    //Logger.warning("[Float]SLOT: " + ac._slot + "; " + half.X + "; " + half.Y + "; " + half.Z);
                    //Logger.warning("[UShort]SLOT: " + ac._slot + "; " + half.X.RawValue + "; " + half.Y.RawValue + "; " + half.Z.RawValue);
                    if (pl != null)
                    {
                        pl.Position = new Half3(info._rotationX, info._rotationY, info._rotationZ);
                        //Logger.warning("X: " + pl.Position.X + "; Y: " + pl.Position.Y + "; Z: " + pl.Position.Z);
                    }
                }
                if (ac._flags.HasFlag(Events.OnLoadObject))
                {
                    contador += 8;
                    a8_MoveSync.Struct info = a8_MoveSync.readSyncInfo(ac, rec, false);
                    a8_MoveSync.writeInfo(sp, info);
                    //Logger.warning("ob: " + (info._objId & 240));
                    if (!room._isBotMode && info._objId != 65535 && (info._spaceFlags.HasFlag(CharaMoves.HELI_STOPPED) || info._spaceFlags.HasFlag(CharaMoves.HELI_IN_MOVE))) //16 entrando no heli em movimento | 96 saindo do heli | 128 entrando no heli parado
                    {
                        bool securityBlock = false;
                        ObjectInfo obj = room.getObject(info._objId);
                        if (obj != null)
                        {
                            if (info._spaceFlags.HasFlag(CharaMoves.HELI_STOPPED))
                            {
                                AnimModel anim = obj._anim;
                                if (anim != null && anim._id == 0)
                                    obj._model.GetAnim(anim._nextAnim, 0, 0, obj);
                            }
                            else if (info._spaceFlags.HasFlag(CharaMoves.HELI_IN_MOVE))
                            {
                                DateTime date = obj._useDate;
                                if (date.ToString("yyMMddHHmm") == "0101010000")
                                    securityBlock = true;
                            }
                            if (!securityBlock)
                                objs.Add(new ObjectHitInfo(3)
                                {
                                    objSyncId = 1,
                                    objId = obj._id,
                                    objLife = obj._life,
                                    _animId1 = 255,
                                    _animId2 = obj._anim != null ? obj._anim._id : 255,
                                    _specialUse = AllUtils.GetDuration(obj._useDate)
                                });
                        }
                    }
                    info = null;
                }
                if (ac._flags.HasFlag(Events.Unk1))
                {
                    contador += 0x10;
                    a10_unk.writeInfo(sp, ac, rec, false);
                }
                if (ac._flags.HasFlag(Events.RadioChat))
                {
                    contador += 0x20;
                    a20_RadioSync.writeInfo(sp, ac, rec, false);
                }
                if (ac._flags.HasFlag(Events.WeaponSync))
                { //.HasFlag(Events.DropWeapon) == FALSE &&
                  //.HasFlag(Events.TakeWeapon) == FALSE
                    contador += 0x40;
                    a40_WeaponSync.Struct info = a40_WeaponSync.ReadInfo(ac, rec, false);
                    a40_WeaponSync.writeInfo(sp, info);
                    if (pl != null)
                    {
                        pl.WeaponClass = (ClassType)info.WeaponClass;
                        pl.WeaponSlot = info.WeaponSlot;
                        pl._character = (CHARACTER_RES_ID)info._charaModelId;
                        room.SyncInfo(objs, 2);
                    }
                }
                if (ac._flags.HasFlag(Events.WeaponRecoil))
                {
                    contador += 0x80;
                    a80_WeaponRecoil.writeInfo(sp, ac, rec, false);
                }
                if (ac._flags.HasFlag(Events.LifeSync))
                {
                    contador += 0x100;
                    a100_LifeSync.writeInfo(sp, rec, false);
                }
                if (ac._flags.HasFlag(Events.Suicide))
                {
                    contador += 0x200;
                    List<a200_SuicideDamage.HitData> hits = a200_SuicideDamage.ReadInfo(rec, false);
                    int basicInfo = 0;
                    if (pl != null)
                    {
                        List<DeathServerData> deaths = new List<DeathServerData>();
                        int ObjIdx = -1;
                        for (int i = 0; i < hits.Count; i++)
                        {
                            a200_SuicideDamage.HitData hit = hits[i];
                            if (pl != null && !pl.isDead && pl._life > 0)
                            {
                                int damage = (int)(hit._hitInfo >> 20);
                                CHARA_DEATH deathType = (CHARA_DEATH)(hit._hitInfo & 15);
                                int killerId = (int)((hit._hitInfo >> 11) & 511);
                                int hitPart = (int)((hit._hitInfo >> 4) & 63);
                                int unk = (int)((hit._hitInfo >> 10) & 1); //0 = User | 1 = Object
                                if (unk == 1)
                                    ObjIdx = killerId;
                                //Logger.warning("Damage: " + damage + "; KillerId: " + killerId + "; hitPart: " + hitPart + "; Unk: " + unk);
                                basicInfo = AllUtils.createItemId(AllUtils.itemClass(hit.WeaponClass), hit._weaponSlot, (int)hit.WeaponClass, hit.WeaponId);

                                pl._life -= damage;
                                if (pl._life <= 0)
                                    DamageManager.SetDeath(deaths, pl, deathType);
                                else
                                    DamageManager.SetHitEffect(objs, pl, deathType, hitPart);
                                objs.Add(new ObjectHitInfo(2)
                                {
                                    objId = pl._slot,
                                    objLife = pl._life,
                                    deathType = deathType,
                                    hitPart = hitPart,
                                    weaponId = basicInfo,
                                    Position = hit.PlayerPos
                                });
                            }
                            else RemoveHit(hits, i--);
                        }
                        if (deaths.Count > 0)
                            Battle_SyncNet.SendDeathSync(room, pl, ObjIdx, basicInfo, deaths);
                        deaths = null;
                    }
                    else hits = new List<a200_SuicideDamage.HitData>();
                    a200_SuicideDamage.writeInfo(sp, hits);
                    hits = null;
                }
                if (ac._flags.HasFlag(Events.Mission))
                {
                    contador += 0x400;
                    a400_Mission.Struct info = a400_Mission.ReadInfo(ac, rec, false, time);

                    //Logger.warning("pl: " + time + ";" + info._plantTime + ";" + info._bombId + ";" + info._bombEnum + ";" + info._bombAll);
                    if (room.Map != null && pl != null && !pl.isDead && info._plantTime > 0 &&
                        !info.BombEnum.HasFlag(BombFlag.Stop))
                    {
                        BombPosition bomb = room.Map.GetBomb(info.BombId);
                        if (bomb != null)
                        {
                            bool isDefuse = info.BombEnum.HasFlag(BombFlag.Defuse);
                            Vector3 BombVec3d;
                            if (isDefuse)
                                BombVec3d = room.BombPosition;
                            else if (info.BombEnum.HasFlag(BombFlag.Start))
                                BombVec3d = bomb.Position;
                            else
                                BombVec3d = new Half3(0, 0, 0);
                            double PlayerDistance = Vector3.Distance(pl.Position, BombVec3d);
                            if ((bomb.Everywhere || PlayerDistance <= 2.0) &&
                                (pl._team == 1 && isDefuse || pl._team == 0 && !isDefuse))
                            {
                                if (pl._C4FTime != info._plantTime)
                                {
                                    pl._C4First = DateTime.Now;
                                    pl._C4FTime = info._plantTime;
                                }
                                double Seconds = (DateTime.Now - pl._C4First).TotalSeconds;

                                float objective = isDefuse ? pl._defuseDuration : pl._plantDuration;

                                if (((time >= info._plantTime + (objective)) || Seconds >= objective) &&
                                    (!room._hasC4 && info.BombEnum.HasFlag(BombFlag.Start) ||
                                        room._hasC4 && isDefuse))
                                {
                                    room._hasC4 = room._hasC4 ? false : true;
                                    info._bombAll |= 2;
                                    a400_Mission.SendC4UseSync(room, pl, info);
                                }
                            }
                        }
                    }
                    a400_Mission.writeInfo(sp, info);
                    info = null;
                }
                if (ac._flags.HasFlag(Events.TakeWeapon))
                {
                    contador += 0x800;
                    a800_WeaponAmmo.writeInfo(sp, ac, rec, false);
                }
                if (ac._flags.HasFlag(Events.DropWeapon))
                {
                    contador += 0x1000;
                    if (room != null && !room._isBotMode)
                    {
                        room._dropCounter++;
                        if (room._dropCounter > Config.maxDrop)
                            room._dropCounter = 0;
                    }
                    a1000_DropWeapon.writeInfo(sp, rec, false, room != null ? room._dropCounter : 0);
                }
                if (ac._flags.HasFlag(Events.FireSync))
                {
                    contador += 0x2000;
                    a2000_FireSync.writeInfo(sp, rec, false);
                }
                if (ac._flags.HasFlag(Events.AIDamage))
                {
                    contador += 0x4000;
                    a4000_BotHitData.writeInfo(sp, rec, false);
                }
                if (ac._flags.HasFlag(Events.NormalDamage))
                {
                    contador += 0x8000;
                    List<a8000_NormalHitData.HitData> hits = a8000_NormalHitData.ReadInfo(rec, false);
                    List<DeathServerData> deaths = new List<DeathServerData>();
                    int basicInfo = 0;
                    if (pl != null)
                    {
                        for (int i = 0; i < hits.Count; i++)
                        {
                            a8000_NormalHitData.HitData hit = hits[i];
                            if (hit.HitEnum == HitType.HelmetProtection || hit.HitEnum == HitType.HeadshotProtection)
                                continue;
                            double HitDistance = Vector3.Distance(hit.StartBullet, hit.EndBullet);
                            //Logger.warning("D: " + PlayersDistance + "; Id: " + hit._weaponId);
                            if (hit._weaponSlot == 2 && (MeleeExceptionsXML.Contains(hit.WeaponId) || HitDistance < 3) || hit._weaponSlot != 2)
                            {
                                int damage = AllUtils.getHitDamageNORMAL(hit._hitInfo),
                                    objId = AllUtils.getHitWho(hit._hitInfo),
                                    hitPart = AllUtils.getHitPart(hit._hitInfo);
                                CHARA_DEATH deathType = CHARA_DEATH.DEFAULT;
                                //Logger.warning("Part: " + hitPart);
                                basicInfo = AllUtils.createItemId(AllUtils.itemClass(hit.WeaponClass), hit._weaponSlot, (int)hit.WeaponClass, hit.WeaponId);
                                ObjectType hitType = AllUtils.getHitType(hit._hitInfo);
                                if (hitType == ObjectType.Object)
                                {
                                    ObjectInfo obj = room.getObject(objId);
                                    ObjModel objM = obj == null ? null : obj._model;
                                    if (objM != null && objM.isDestroyable)
                                    {
                                        if (obj._life > 0)
                                        {
                                            obj._life -= damage;
                                            if (obj._life <= 0)
                                            {
                                                obj._life = 0;
                                                DamageManager.BoomDeath(room, pl, basicInfo, deaths, objs, hit.BoomPlayers);
                                            }
                                            obj.DestroyState = objM.CheckDestroyState(obj._life);
                                            DamageManager.SabotageDestroy(room, pl, objM, obj, damage);
                                            objs.Add(new ObjectHitInfo(objM._updateId)
                                            {
                                                objId = obj._id,
                                                objLife = obj._life,
                                                killerId = ac._slot,
                                                objSyncId = objM._needSync ? 1 : 0,
                                                _specialUse = AllUtils.GetDuration(obj._useDate),
                                                _animId1 = objM._anim1,
                                                _animId2 = obj._anim != null ? obj._anim._id : 255,
                                                _destroyState = obj.DestroyState
                                            });
                                        }
                                    }
                                    else if (Config.sendFailMsg && objM == null)
                                    {
                                        Logger.warning("[Fire] Obj: " + objId + "; Mapa: " + room._mapId + "; objeto inválido.");
                                        pl.LogPlayerPos(hit.EndBullet);
                                    }
                                }
                                else if (hitType == ObjectType.User)
                                {
                                    Player player;
                                    if (room.getPlayer(objId, out player) && pl.RespawnLogicIsValid() && !pl.isDead && !player.isDead && !player.Immortal)
                                    {
                                        if ((int)hitPart == 29)
                                            deathType = CHARA_DEATH.HEADSHOT;

                                        if (room.stageType == 8 && deathType != CHARA_DEATH.HEADSHOT)
                                            damage = 1;
                                        else if (room.stageType == 7 && deathType == CHARA_DEATH.HEADSHOT)
                                        {
                                            if (room.LastRound == 1 && player._team == 0 ||
                                                room.LastRound == 2 && player._team == 1)
                                                damage = (damage / 10);
                                        }
                                        else if (room.stageType == 13)
                                            damage = 200;
                                        if (Config.useHitMarker)
                                            Battle_SyncNet.SendHitMarkerSync(room, pl, (int)deathType, (int)hit.HitEnum, damage);
                                        DamageManager.SimpleDeath(deaths, objs, pl, player, damage, basicInfo, hitPart, deathType);
                                    }
                                    else RemoveHit(hits, i--);
                                }
                                else if (hitType == ObjectType.UserObject)
                                {
                                    int ownerSlot = (objId >> 4);
                                    int grenadeMapId = (objId & 15);
                                    //Logger.warning("[UserObject] Dano: " + damage + "; Player: " + ownerSlot + "; MapObj: " + grenadeMapId);
                                }
                                else
                                {
                                    Logger.warning("[Warning] A new hit type: (" + hitType + "/" + (int)hitType + "); by slot: " + ac._slot);
                                    Logger.warning("[Warning] BoomPlayers: " + hit._boomInfo + ";" + hit.BoomPlayers.Count);
                                }
                            }
                            else
                                RemoveHit(hits, i--);
                        }
                        if (deaths.Count > 0)
                            Battle_SyncNet.SendDeathSync(room, pl, 255, basicInfo, deaths);
                    }
                    else hits = new List<a8000_NormalHitData.HitData>();
                    a8000_NormalHitData.writeInfo(sp, hits);
                    deaths = null;
                    hits = null;
                }
                if (ac._flags.HasFlag(Events.BoomDamage))
                {
                    contador += 0x10000;
                    List<a10000_BoomHitData.HitData> hits = a10000_BoomHitData.ReadInfo(rec, false);
                    List<DeathServerData> deaths = new List<DeathServerData>();
                    int basicInfo = 0;
                    if (pl != null)
                    {
                        int idx = -1;
                        for (int i = 0; i < hits.Count; i++)
                        {
                            a10000_BoomHitData.HitData hit = hits[i];
                            int damage = AllUtils.getHitDamageNORMAL(hit._hitInfo),
                                objId = AllUtils.getHitWho(hit._hitInfo),
                                hitPart = AllUtils.getHitPart(hit._hitInfo);
                            basicInfo = AllUtils.createItemId(AllUtils.itemClass(hit.WeaponClass), hit._weaponSlot, (int)hit.WeaponClass, hit.WeaponId);
                            ObjectType hitType = AllUtils.getHitType(hit._hitInfo);
                            if (hitType == ObjectType.Object)
                            {
                                ObjectInfo obj = room.getObject(objId);
                                ObjModel objM = obj == null ? null : obj._model;
                                if (objM != null && objM.isDestroyable && obj._life > 0)
                                {
                                    obj._life -= damage;
                                    if (obj._life <= 0)
                                    {
                                        obj._life = 0;
                                        DamageManager.BoomDeath(room, pl, basicInfo, deaths, objs, hit.BoomPlayers);
                                    }
                                    obj.DestroyState = objM.CheckDestroyState(obj._life);
                                    DamageManager.SabotageDestroy(room, pl, objM, obj, damage);
                                    if (damage > 0)
                                        objs.Add(new ObjectHitInfo(objM._updateId)
                                        {
                                            objId = obj._id,
                                            objLife = obj._life,
                                            killerId = ac._slot,
                                            objSyncId = objM._needSync ? 1 : 0,
                                            _animId1 = objM._anim1,
                                            _animId2 = obj._anim != null ? obj._anim._id : 255,
                                            _destroyState = obj.DestroyState,
                                            _specialUse = AllUtils.GetDuration(obj._useDate),
                                        });
                                }
                                else if (Config.sendFailMsg && objM == null)
                                {
                                    Logger.warning("[Boom] Obj: " + objId + "; Mapa: " + room._mapId + "; objeto inválido.");
                                    pl.LogPlayerPos(hit.HitPos);
                                }
                            }
                            else if (hitType == ObjectType.User)
                            {
                                idx++;
                                Player player;
                                if (damage > 0 && room.getPlayer(objId, out player) && pl.RespawnLogicIsValid() && !player.isDead && !player.Immortal)
                                {
                                    if (hit._deathType == 10)
                                    {
                                        player._life += damage;
                                        player.CheckLifeValue();
                                    }
                                    else if (hit._deathType == 2 && ClassType.Dino != hit.WeaponClass && (idx % 2 == 0))
                                    {
                                        int valor = damage;
                                        damage = (int)Math.Ceiling(damage / 2.7);// + 14;
                                       // Console.WriteLine("The server is reduced from " + valor + " to " + damage + " damage by Explosion.");
                                        player._life -= damage;
                                        if (player._life <= 0)
                                            DamageManager.SetDeath(deaths, player, (CHARA_DEATH)hit._deathType);
                                        else
                                            DamageManager.SetHitEffect(objs, player, pl, (CHARA_DEATH)hit._deathType, hitPart);
                                    }
                                    else
                                    {
                                        player._life -= damage;
                                        if (player._life <= 0)
                                            DamageManager.SetDeath(deaths, player, (CHARA_DEATH)hit._deathType);
                                        else
                                            DamageManager.SetHitEffect(objs, player, pl, (CHARA_DEATH)hit._deathType, hitPart);
                                    }
                                    if (damage > 0)
                                    {
                                        if (Config.useHitMarker)
                                            Battle_SyncNet.SendHitMarkerSync(room, pl, hit._deathType, (int)hit.HitEnum, damage);
                                        objs.Add(new ObjectHitInfo(2)
                                        {
                                            objId = player._slot,
                                            objLife = player._life,
                                            deathType = (CHARA_DEATH)hit._deathType,
                                            weaponId = basicInfo,
                                            hitPart = hitPart
                                        });
                                    }
                                }
                                else RemoveHit(hits, i--);
                            }
                            else if (hitType == ObjectType.UserObject)
                            {
                                int ownerSlot = (objId >> 4);
                                int grenadeMapId = (objId & 15);
                               // Logger.warning("[UserObject2] Dano: " + damage + "; Player: " + ownerSlot + "; MapObj: " + grenadeMapId);
                            }
                            else
                                Logger.warning("Grenade BOOM, new hit type: (" + hitType + "/" + (int)hitType + ")");
                        }
                        if (deaths.Count > 0)
                            Battle_SyncNet.SendDeathSync(room, pl, 255, basicInfo, deaths);
                    }
                    else hits = new List<a10000_BoomHitData.HitData>();
                    a10000_BoomHitData.writeInfo(sp, hits);
                    deaths = null;
                    hits = null;
                }
                if (ac._flags.HasFlag(Events.Death))
                {
                    contador += 0x40000;
                    a40000_DeathData.writeInfo(sp, ac, rec, false);
                }
                if (ac._flags.HasFlag(Events.SufferingDamage))
                {
                    contador += 0x80000;
                    a80000_SufferingDamage.writeInfo(sp, ac, rec, false);
                }
                if (ac._flags.HasFlag(Events.PassPortal))
                {
                    contador += 0x100000;
                    a100000_PassPortal.Struct info = a100000_PassPortal.ReadInfo(ac, rec, false);
                    a100000_PassPortal.writeInfo(sp, info);
                    if (pl != null && !pl.isDead)
                        a100000_PassPortal.SendPassSync(room, pl, info);
                    info = null;
                }
                EventsData = sp.mstream.ToArray();
                if (contador != (uint)ac._flags)
                    Logger.warning("[" + (uint)ac._flags + "|" + ((uint)ac._flags - contador) + ", BUF ANORMAL3: " + BitConverter.ToString(data));
                return objs;
            }
        }
        public void CheckDataFlags(ActionModel ac, PacketModel packet)
        {
            Events flags = ac._flags;
            if (!flags.HasFlag(Events.WeaponSync) || packet._opcode == 4)
                return;
            if ((flags & (Events.TakeWeapon | Events.DropWeapon)) > 0)
            {
                ac._flags -= Events.WeaponSync;
               //Logger.warning("Flag ajustada. (" + (uint)ac._flags + ")");
               //Logger.warning("Buffer: " + BitConverter.ToString(ac._data));
               //Logger.warning("Opcode: " + packet._opcode);
            }
        }
        public byte[] WriteActionBytes(byte[] data, Room room, float time, PacketModel packet)
        {
            ReceivePacket p = new ReceivePacket(data);
            List<ObjectHitInfo> objs = new List<ObjectHitInfo>();
            using (SendPacket s = new SendPacket())
            {
                for (int i = 0; i < 16; i++)
                {
                    ActionModel ac = new ActionModel();
                    try
                    {
                        bool exception;
                        ac._type = (P2P_SUB_HEAD)p.readC(out exception);
                        if (exception) break;
                        ac._slot = p.readUH();
                        ac._lengthData = p.readUH();
                        if (ac._lengthData == 65535)
                            break;
                        s.writeC((byte)ac._type);
                        s.writeH(ac._slot);
                        s.writeH(ac._lengthData);
                        if (ac._type == P2P_SUB_HEAD.GRENADE)
                            code1_GrenadeSync.writeInfo(s, p, false);
                        else if (ac._type == P2P_SUB_HEAD.DROPEDWEAPON)
                            code2_WeaponSync.writeInfo(s, p, false);
                        else if (ac._type == P2P_SUB_HEAD.OBJECT_STATIC)
                            code3_ObjectStatic.writeInfo(s, p, false);
                        else if (ac._type == P2P_SUB_HEAD.OBJECT_ANIM)
                            code6_ObjectAnim.writeInfo(s, p, false);
                        else if (ac._type == P2P_SUB_HEAD.STAGEINFO_OBJ_STATIC)
                            code9_StageInfoObjStatic.writeInfo(s, p, false);
                        else if (ac._type == P2P_SUB_HEAD.STAGEINFO_OBJ_ANIM)
                            code12_StageObjAnim.writeInfo(s, p, false);
                        else if (ac._type == P2P_SUB_HEAD.CONTROLED_OBJECT)
                            code13_ControledObj.writeInfo(s, p, false);
                        else if (ac._type == P2P_SUB_HEAD.USER || ac._type == P2P_SUB_HEAD.STAGEINFO_CHARA)
                        {
                            ac._flags = (Events)p.readUD();
                            ac._data = p.readB(ac._lengthData - 9);
                            CheckDataFlags(ac, packet);
                            byte[] result;
                            objs.AddRange(getNewActions(ac, room, time, out result));
                            s.GoBack(2);
                            s.writeH((ushort)(result.Length + 9));
                            s.writeD((uint)ac._flags);
                            s.writeB(result);
                            if (ac._data.Length == 0 && (ac._lengthData - 9 != 0))
                                break;
                        }
                        else
                        {
                            Logger.warning("[New user packet type '" + ac._type + "' or '" + (int)ac._type + "']: " + BitConverter.ToString(data));
                            throw new Exception("Unknown action type");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.warning("[WriteActionBytes]\r\n" + ex.ToString());
                        Logger.warning("[WAB] Data: " + BitConverter.ToString(data));
                        objs = new List<ObjectHitInfo>();
                        break;
                    }
                }
                if (objs.Count > 0)
                    s.writeB(Packet4Creator.getCode4SyncData(objs));
                objs = null;
                return s.mstream.ToArray();
            }
        }
        private void Send(byte[] data, IPEndPoint ip)
        {
            _client.Send(data, data.Length, ip);
        }
    }
}