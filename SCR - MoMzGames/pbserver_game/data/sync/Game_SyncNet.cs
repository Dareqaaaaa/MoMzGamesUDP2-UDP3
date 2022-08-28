using Core;
using Core.managers.events;
using Core.managers.server;
using Core.models.account;
using Core.models.enums;
using Core.models.enums.flags;
using Core.models.enums.friends;
using Core.models.enums.missions;
using Core.models.room;
using Core.models.servers;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.client_side;
using Game.data.utils;
using Game.data.xml;
using Game.global.serverpacket;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Game.data.sync
{
    public static class Game_SyncNet
    {
        private static DateTime LastSyncCount;
        public static UdpClient udp;
        public static void Start()
        {
            try
            {
                udp = new UdpClient(ConfigGS.syncPort);
                uint IOC_IN = 0x80000000;
                uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                udp.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                new Thread(read).Start();
            }
            catch (Exception e)
            {
                Logger.error(e.ToString());
            }
        }
        public static void read()
        {
            try
            {
                udp.BeginReceive(new AsyncCallback(recv), null);
            }
            catch
            {
            }
        }
        private static void recv(IAsyncResult res)
        {
            if (GameManager.ServerIsClosed)
                return;
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8000);
            byte[] received = udp.EndReceive(res, ref RemoteIpEndPoint);
            Thread.Sleep(5);
            new Thread(read).Start();
            if (received.Length >= 2)
                LoadPacket(received);
        }
        private static void LoadPacket(byte[] buffer)
        {
            ReceiveGPacket p = new ReceiveGPacket(buffer);
            short opcode = p.readH();
            try
            {
                if (opcode == 1)
                    Net_Room_Pass_Portal.Load(p);
                else if (opcode == 2) //Bomba
                    Net_Room_C4.Load(p);
                else if (opcode == 3) //Death
                    Net_Room_Death.Load(p);
                else if (opcode == 4)
                    Net_Room_HitMarker.Load(p);
                else if (opcode == 5)
                    Net_Room_Sabotage_Sync.Load(p);
                else if (opcode == 10)
                {
                    Account player = AccountManager.getAccount(p.readQ(), true);
                    if (player != null)
                    {
                        player.SendPacket(new AUTH_ACCOUNT_KICK_PAK(1));
                        player.SendPacket(new SERVER_MESSAGE_ERROR_PAK(0x80001000));
                        player.Close(1000);
                    }
                }
                else if (opcode == 11) //Request to sync a specific friend or clan info
                {
                    int type = p.readC();
                    int isConnect = p.readC();
                    Account player = AccountManager.getAccount(p.readQ(), 0);
                    if (player != null)
                    {
                        Account friendAcc = AccountManager.getAccount(p.readQ(), true);
                        if (friendAcc != null)
                        {
                            FriendState state = isConnect == 1 ? FriendState.Online : FriendState.Offline;
                            if (type == 0)
                            {
                                int idx = -1;
                                Friend friend = friendAcc.FriendSystem.GetFriend(player.player_id, out idx);
                                if (idx != -1 && friend != null && friend.state == 0)
                                    friendAcc.SendPacket(new FRIEND_UPDATE_PAK(FriendChangeState.Update, friend, state, idx));
                            }
                            else friendAcc.SendPacket(new CLAN_MEMBER_INFO_CHANGE_PAK(player, state));
                        }
                    }
                }
                else if (opcode == 13)
                {
                    long playerId = p.readQ();
                    byte type = p.readC();
                    byte[] data = p.readB(p.readUH());
                    Account player = AccountManager.getAccount(playerId, true);
                    if (player != null)
                    {
                        if (type == 0)
                            player.SendPacket(data);
                        else
                            player.SendCompletePacket(data);
                    }
                }
                else if (opcode == 15)
                {
                    int serverId = p.readD();
                    int count = p.readD();
                    GameServerModel gs = ServersXML.getServer(serverId);
                    if (gs != null)
                        gs._LastCount = count;
                }
                else if (opcode == 16)
                    Net_Clan_Sync.Load(p);
                else if (opcode == 17)
                    Net_Friend_Sync.Load(p);
                else if (opcode == 18)
                    Net_Inventory_Sync.Load(p);
                else if (opcode == 19)
                    Net_Player_Sync.Load(p);
                else if (opcode == 20)
                    Net_Server_Warning.LoadGMWarning(p);
                else if (opcode == 21)
                    Net_Clan_Servers_Sync.Load(p);
                else if (opcode == 22)
                    Net_Server_Warning.LoadShopRestart(p);
                else if (opcode == 23)
                    Net_Server_Warning.LoadServerUpdate(p);
                else if (opcode == 24)
                    Net_Server_Warning.LoadShutdown(p);
                else if (opcode == 31)
                {
                    int type = p.readC();
                    EventLoader.ReloadEvent(type);
                    Logger.warning("[Game_SyncNet] Evento re-carregado.");
                }
                else if (opcode == 32)
                {
                    int config = p.readC();
                    ServerConfigSyncer.GenerateConfig(config);
                    Logger.warning("[Game_SyncNet] Configurações (DB) resetadas.");
                }
                else Logger.warning("[Game_SyncNet] Tipo de conexão não encontrada: " + opcode);
            }
            catch (Exception ex)
            {
                Logger.error("[Crash/Game_SyncNet] Tipo: " + opcode + "\r\n" + ex.ToString());
                if (p != null)
                    Logger.error("COMP: " + BitConverter.ToString(p.getBuffer()));
            }
        }
        /// <summary>
        /// Envia as informações com pedido de sincronia para o UDP.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="effects"></param>
        /// <param name="room"></param>
        public static void SendUDPPlayerSync(Room room, SLOT slot, CupomEffects effects, int type)
        {
            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(1);
                pk.writeD(room.UniqueRoomId);
                pk.writeD((room.mapId * 16) + room.room_type);
                pk.writeQ(room.StartTick); //tick
                pk.writeC((byte)type);
                pk.writeC((byte)room.rodada);
                pk.writeC((byte)slot._id);
                pk.writeC((byte)slot.spawnsCount);
                pk.writeC(BitConverter.GetBytes(slot._playerId)[0]);
                if (type == 0 || type == 2)
                    WriteCharaInfo(pk, room, slot, effects);
                SendPacket(pk.mstream.ToArray(), room.UDPServer.Connection);
            }
        }
        private static void WriteCharaInfo(SendGPacket pk, Room room, SLOT slot, CupomEffects effects)
        {
            int charaId = 0;
            if ((RoomType)room.room_type == RoomType.Boss || (RoomType)room.room_type == RoomType.Cross_Counter)
            {
                if (room.rodada == 1 && slot._team == 1 ||
                    room.rodada == 2 && slot._team == 0)
                    charaId = room.rodada == 2 ? slot._equip._red : slot._equip._blue;
                else if (room.TRex == slot._id)
                    charaId = -1;
                else
                    charaId = slot._equip._dino;
            }
            else charaId = slot._team == 0 ? slot._equip._red : slot._equip._blue;
            int HPBonus = 0;
            if (effects.HasFlag(CupomEffects.Ketupat))
                HPBonus += 10;
            if (effects.HasFlag(CupomEffects.HP5))
                HPBonus += 5;
            if (effects.HasFlag(CupomEffects.HP10))
                HPBonus += 10;
            if (charaId == -1)
            {
                pk.writeC(255);
                pk.writeH(65535);
            }
            else
            {
                pk.writeC((byte)ComDiv.getIdStatics(charaId, 2));
                pk.writeH((short)ComDiv.getIdStatics(charaId, 4));
            }
            pk.writeC((byte)HPBonus);
            pk.writeC(effects.HasFlag(CupomEffects.C4SpeedKit));
        }
        public static void SendUDPRoundSync(Room room)
        {
            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(3);
                pk.writeD(room.UniqueRoomId);
                pk.writeD((room.mapId * 16) + room.room_type);
                pk.writeC((byte)room.rodada);
                SendPacket(pk.mstream.ToArray(), room.UDPServer.Connection);
            }
        }
        public static GameServerModel GetServer(AccountStatus status)
        {
            return GetServer(status.serverId);
        }
        public static GameServerModel GetServer(int serverId)
        {
            if (serverId == 255 || serverId == ConfigGS.serverId)
                return null;
            return ServersXML.getServer(serverId);
        }
        public static void UpdateGSCount(int serverId)
        {
            try
            {
                double pingMS = (DateTime.Now - LastSyncCount).TotalSeconds;
                if (pingMS < 5)
                    return;

                LastSyncCount = DateTime.Now;
                int players = 0;
                foreach (Channel ch in ChannelsXML._channels)
                    players += ch._players.Count;
                foreach (GameServerModel gs in ServersXML._servers)
                {
                    if (gs._id == serverId)
                        gs._LastCount = players;
                    else
                    {
                        using (SendGPacket pk = new SendGPacket())
                        {
                            pk.writeH(15);
                            pk.writeD(serverId);
                            pk.writeD(players);
                            SendPacket(pk.mstream.ToArray(), gs.Connection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.warning("[Game_SyncNet.UpdateGSCount] " + ex.ToString());
            }
        }
        public static void SendBytes(long playerId, SendPacket sp, int serverId)
        {
            if (sp == null)
                return;
            GameServerModel gs = GetServer(serverId);
            if (gs == null)
                return;

            byte[] data = sp.GetBytes("Game_SyncNet.SendBytes");
            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(13);
                pk.writeQ(playerId);
                pk.writeC(0);
                pk.writeH((ushort)data.Length);
                pk.writeB(data);
                SendPacket(pk.mstream.ToArray(), gs.Connection);
            }
        }
        public static void SendBytes(long playerId, byte[] buffer, int serverId)
        {
            if (buffer.Length == 0)
                return;
            GameServerModel gs = GetServer(serverId);
            if (gs == null)
                return;

            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(13);
                pk.writeQ(playerId);
                pk.writeC(0);
                pk.writeH((ushort)buffer.Length);
                pk.writeB(buffer);
                SendPacket(pk.mstream.ToArray(), gs.Connection);
            }
        }
        public static void SendCompleteBytes(long playerId, byte[] buffer, int serverId)
        {
            if (buffer.Length == 0)
                return;
            GameServerModel gs = GetServer(serverId);
            if (gs == null)
                return;

            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(13);
                pk.writeQ(playerId);
                pk.writeC(1);
                pk.writeH((ushort)buffer.Length);
                pk.writeB(buffer);
                SendPacket(pk.mstream.ToArray(), gs.Connection);
            }
        }
        public static void SendPacket(byte[] data, IPEndPoint ip)
        {
            udp.Send(data, data.Length, ip);
        }
        public static void genDeath(Room room, SLOT killer, FragInfos kills, bool isSuicide)
        {
            int score;
            bool isBotMode = room.isBotMode();
            Net_Room_Death.RegistryFragInfos(room, killer, out score, isBotMode, isSuicide, kills);
            if (isBotMode)
            {
                killer.Score += killer.killsOnLife + room.IngameAiLevel + score;
                if (killer.Score > 65535)
                {
                    killer.Score = 65535;
                    Logger.warning("[PlayerId: " + killer._id + "] chegou a pontuação máxima do modo BOT.");
                }
                kills.Score = killer.Score;
            }
            else
            {
                killer.Score += score;
                AllUtils.CompleteMission(room, killer, kills, MISSION_TYPE.NA, 0);
                kills.Score = score;
            }
            using (BATTLE_DEATH_PAK packet = new BATTLE_DEATH_PAK(room, kills, killer, isBotMode))
                room.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
            Net_Room_Death.EndBattleByDeath(room, killer, isBotMode, isSuicide);
        }
    }
}