using Battle.config;
using System;
using System.Net;
using System.Net.Sockets;

namespace Battle.network
{
    public class BattleManager
    {
        private static UdpClient udpClient;
        public static void init()
        {
            try
            {
                udpClient = new UdpClient();
                //udpClient.Ttl = 255;
                //Logger.warning("P: " + udpClient.ExclusiveAddressUse);
                //udpClient.ExclusiveAddressUse = false;
                uint IOC_IN = 0x80000000;
                uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                udpClient.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(Config.hosIp), Config.hosPort);
                var s = new UdpState(localEP, udpClient);
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Client.Bind(localEP);
                udpClient.BeginReceive(gerenciaRetorno, s);
                Logger.warning("[Aviso] Portas abertas! (" + DateTime.Now.ToString("yy/MM/dd HH:mm:ss") + ")");
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString() + "\r\nOcorreu um erro ao listar as conexões UDP!!");
            }
        }
        private static void read(UdpState state)
        {
            try
            {
                udpClient.BeginReceive(new AsyncCallback(gerenciaRetorno), state);
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
        }
        private static void gerenciaRetorno(IAsyncResult ar)
        {
            if (!ar.IsCompleted)
                Logger.warning("ar is not completed.");
            ar.AsyncWaitHandle.WaitOne(5000);
            DateTime now = DateTime.Now;
            IPEndPoint recEP = new IPEndPoint(IPAddress.Any, 0);
            UdpClient c = (UdpClient)((UdpState)ar.AsyncState).c;
            IPEndPoint e = (IPEndPoint)((UdpState)ar.AsyncState).e;
            try
            {
                byte[] buffer = c.EndReceive(ar, ref recEP);
                if (buffer.Length >= 22)
                    new BattleHandler(udpClient, buffer, recEP, now);
                else
                    Logger.warning("No length (22) buffer: " + BitConverter.ToString(buffer));
            }
            catch (Exception ex)
            {
                Logger.warning("[Exception]: " + recEP.Address + ":" + recEP.Port);
                Logger.warning(ex.ToString());
            }
            UdpState s = new UdpState(e, c);
            read(s);
        }
        public static void Send(byte[] data, IPEndPoint ip)
        {
            udpClient.Send(data, data.Length, ip);
        }
        private class UdpState : Object
        {
            public UdpState(IPEndPoint e, UdpClient c) { this.e = e; this.c = c; }
            public IPEndPoint e;
            public UdpClient c;
        }
    }
}