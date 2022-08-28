using Auth.data.managers;
using Auth.data.model;
using Auth.global.serverpacket;
using Core;
using Core.managers;
using Core.server;
using Core.xml;
using System;
using System.Net.Sockets;
using System.Threading;

namespace Auth.data.sync.client_side
{
    public static class Net_Server_Warning
    {
        public static void LoadGMWarning(ReceiveGPacket p)
        {
            string login = p.readS(p.readC());
            string pass = p.readS(p.readC());
            string msg = p.readS(p.readH());
            string passEnc = ComDiv.gen5(pass);
            Account pl = AccountManager.getInstance().getAccountDB(login, passEnc, 2, 0);
            if (pl != null && (int)pl.access > 3)
            {
                int count = 0;
                using (SERVER_MESSAGE_ANNOUNCE_PAK packet = new SERVER_MESSAGE_ANNOUNCE_PAK(msg))
                    count = LoginManager.SendPacketToAllClients(packet);
                Logger.warning("[SM] Mensagem enviada a " + count + " jogadores: " + msg + "; by Login: '" + login + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
                Logger.LogCMD("[Via SM] Mensagem enviada a " + count + " jogadores: " + msg + "; by Login: '" + login + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
            }
        }
        public static void LoadShopRestart(ReceiveGPacket p)
        {
            int type = p.readC();
            ShopManager.Reset();
            ShopManager.Load(type);
            Logger.warning("[SM] Shop reiniciada. (Type: " + type + ")");
            Logger.LogCMD("[Via SM] Shop reiniciada. (Type: " + type + "); Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
        }
        public static void LoadServerUpdate(ReceiveGPacket p)
        {
            int serverId = p.readC();
            ServersXML.UpdateServer(serverId);
            Logger.warning("[SM] Servidor " + serverId + " atualizado.");
            Logger.LogCMD("[Via SM] Servidor " + serverId + " atualizado.; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
        }
        public static void LoadShutdown(ReceiveGPacket p)
        {
            string login = p.readS(p.readC());
            string pass = p.readS(p.readC());
            string passEnc = ComDiv.gen5(pass);
            Account pl = AccountManager.getInstance().getAccountDB(login, passEnc, 2, 0);
            if (pl != null && pl.password == passEnc && (int)pl.access >= 4)
            {
                int count = 0;
                foreach (LoginClient client in LoginManager._socketList.Values)
                {
                    client._client.Shutdown(SocketShutdown.Both);
                    client._client.Close(10000);
                    count++;
                }
                Logger.warning("[SM] Jogadores Desconectados: " + count + ". (By: " + login + ")");
                LoginManager.ServerIsClosed = true;
                LoginManager.mainSocket.Close(5000);
                Logger.warning("[SM] 1/2 Step");
                Thread.Sleep(5000);
                Auth_SyncNet.udp.Close();
                Logger.warning("[SM] 2/2 Step.");
                foreach (LoginClient client in LoginManager._socketList.Values)
                {
                    client.Close(0, true);
                }
                Logger.warning("[SM] Servidor foi completamente desligado.");
                Logger.LogCMD("[Via SM] Shutdowned Server: " + count + " jogadores desconectados; by Login: '" + login + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
            }
        }
    }
}