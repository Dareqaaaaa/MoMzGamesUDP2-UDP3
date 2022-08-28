using Core;
using Core.managers;
using Core.server;
using Core.xml;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Threading;

namespace Game.data.sync.client_side
{
    public class Net_Server_Warning
    {
        public static void LoadGMWarning(ReceiveGPacket p)
        {
            string login = p.readS(p.readC());
            string pass = p.readS(p.readC());
            string msg = p.readS(p.readH());

            Account pl = AccountManager.getAccount(login, 0, 0);
            if (pl != null && pl.password == ComDiv.gen5(pass) && (int)pl.access >= 4)
            {
                int count = 0;
                using (SERVER_MESSAGE_ANNOUNCE_PAK packet = new SERVER_MESSAGE_ANNOUNCE_PAK(msg))
                    count = GameManager.SendPacketToAllClients(packet);
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

            Account pl = AccountManager.getAccount(login, 0, 0);
            if (pl != null && pl.password == ComDiv.gen5(pass) && (int)pl.access >= 5)
            {
                int count = 0;
                foreach (GameClient client in GameManager._socketList.Values)
                {
                    client._client.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                    client.Close(5000);
                    count++;
                }
                Logger.warning("[SM] Jogadores Desconectados: " + count + ". (By: " + login + ")");
                GameManager.ServerIsClosed = true;
                GameManager.mainSocket.Close(5000);
                Logger.warning("[SM] 1/2 Step");
                Thread.Sleep(5000);
                Game_SyncNet.udp.Close();
                Logger.warning("[SM] 2/2 Step.");
                foreach (GameClient client in GameManager._socketList.Values)
                {
                    client.Close(0);
                }
                Logger.warning("[SM] Servidor foi completamente desligado.");
                Logger.LogCMD("[Via SM] Shutdowned Server: " + count + " jogadores desconectados; by Login: '" + login + "'; Date: '" + DateTime.Now.ToString("dd/MM/yy HH:mm") + "'");
            }
        }
    }
}