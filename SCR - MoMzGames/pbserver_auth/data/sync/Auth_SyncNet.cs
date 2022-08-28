/*
 * Arquivo: Auth_SyncNet.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 10/07/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.managers;
using Auth.data.model;
using Auth.data.sync.client_side;
using Auth.global.serverpacket;
using Core;
using Core.managers.events;
using Core.managers.server;
using Core.models.account;
using Core.models.enums.friends;
using Core.models.servers;
using Core.server;
using Core.xml;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Auth.data.sync
{
    public class Auth_SyncNet
    {
        private static DateTime LastSyncCount;
        public static UdpClient udp;
        public static void Start()
        {
            try
            {
                udp = new UdpClient(ConfigGA.syncPort);
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
            catch// (Exception ex)
            {
                //Logger.warning(ex.ToString());
                //close(false, 0);
            }
        }
        private static void recv(IAsyncResult res)
        {
            if (LoginManager.ServerIsClosed)
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
            if (opcode == 11) //Request to sync a specific friend or clan info
            {
                int type = p.readC();
                int isConnect = p.readC();
                Account player = AccountManager.getInstance().getAccount(p.readQ(), true);
                if (player != null)
                {
                    Account friend = AccountManager.getInstance().getAccount(p.readQ(), true);
                    if (friend != null)
                    {
                        FriendState state = isConnect == 1 ? FriendState.Online : FriendState.Offline;
                        if (type == 0)
                        {
                            int idx = -1;
                            Friend frP = friend.FriendSystem.GetFriend(player.player_id, out idx);
                            if (idx != -1 && frP != null)
                                friend.SendPacket(new FRIEND_UPDATE_PAK(FriendChangeState.Update, frP, state, idx));
                        }
                        else friend.SendPacket(new CLAN_MEMBER_INFO_CHANGE_PAK(player, state));
                    }
                }
            }
            else if (opcode == 13)
            {
                long playerId = p.readQ();
                byte type = p.readC();
                byte[] data = p.readB(p.readUH());
                Account player = AccountManager.getInstance().getAccount(playerId, true);
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
            else if (opcode == 19)
                Net_Player_Sync.Load(p);
            else if (opcode == 20)
                Net_Server_Warning.LoadGMWarning(p);
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
                Logger.warning("[Auth_SyncNet] Evento re-carregado.");
                Logger.LogCMD("[Via SM] Evento re-carregado; Type: "+type+"; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
                Logger.LogCMD("[Via SM] Evento re-carregado; Type: " + type + "; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
            }
            else if (opcode == 32)
            {
                int config = p.readC();
                ServerConfigSyncer.GenerateConfig(config);
                Logger.warning("[Auth_SyncNet] Configurações (DB) Recarregadas.; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
                Logger.LogCMD("[Via SM] Configurações (DB) Recarregadas.; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
            }
            else Logger.warning("[Auth_SyncNet] Tipo de conexão não encontrada: " + opcode);
        }
        public static void UpdateGSCount(int serverId)
        {
            try
            {
                double pingMS = (DateTime.Now - LastSyncCount).TotalSeconds;
                if (pingMS < 2.5)
                    return;

                LastSyncCount = DateTime.Now;
                int players = LoginManager._socketList.Count;
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
                Logger.warning(ex.ToString());
            }
        }
        public static void SendLoginKickInfo(Account player)
        {
            int serverId = player._status.serverId;
            if (serverId != 255 && serverId != 0)
            {
                GameServerModel gs = ServersXML.getServer(serverId);
                if (gs == null)
                    return;

                using (SendGPacket pk = new SendGPacket())
                {
                    pk.writeH(10);
                    pk.writeQ(player.player_id);
                    SendPacket(pk.mstream.ToArray(), gs.Connection);
                }
            }
            else
                player.setOnlineStatus(false);
        }
        public static void SendPacket(byte[] data, IPEndPoint ip)
        {
            udp.Send(data, data.Length, ip);
        }
    }
}