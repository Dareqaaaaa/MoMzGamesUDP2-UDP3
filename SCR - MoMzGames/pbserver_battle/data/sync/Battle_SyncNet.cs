using Battle.config;
using Battle.data.models;
using Battle.data.sync.client_side;
using Battle.network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Battle.data.sync
{
    public class Battle_SyncNet
    {
        private static UdpClient udp;
        public static void Start()
        {
            try
            {
                udp = new UdpClient(Config.syncPort);
                uint IOC_IN = 0x80000000;
                uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                udp.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                new Thread(read).Start();
                //Logger.warning("[Aviso] Server sync port: " + Config.syncPort);
            }
            catch (Exception e)
            {
                Logger.warning(e.ToString());
            }
        }
        public static void read()
        {
            try
            {
                udp.BeginReceive(new AsyncCallback(recv), null);
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
        }
        private static void recv(IAsyncResult res)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8000);
            byte[] received = udp.EndReceive(res, ref RemoteIpEndPoint);

            new Thread(read).Start();
            if (received.Length >= 2)
                LoadPacket(received);
        }
        private static void LoadPacket(byte[] buffer)
        {
            ReceivePacket p = new ReceivePacket(buffer);
            short opcode = p.readH();
            if (opcode == 1)
                RespawnSync.Load(p);
            else if (opcode == 2)
                RemovePlayerSync.Load(p);
            else if (opcode == 3)
            {
                uint UniqueRoomId = p.readUD();
                int gen2 = p.readD();
                int round = p.readC();
                Room room = RoomsManager.getRoom(UniqueRoomId, gen2);
                if (room != null)
                    room._serverRound = round;
            }
        }
        public static void SendPortalPass(Room room, Player pl, int portalIdx)
        {
            if (room.stageType != 7)
                return;
            pl._life = pl._maxLife; 
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            using (SendPacket p = new SendPacket())
            {
                p.writeH(1);
                p.writeH((short)room._roomId);
                p.writeH((short)room._channelId);
                p.writeC((byte)pl._slot);
                p.writeC((byte)portalIdx);
                SendData(room, s, p.mstream.ToArray());
            }
        }
        public static void SendDeathSync(Room room, Player killer, int objId, int weaponId, List<DeathServerData> deaths)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            using (SendPacket p = new SendPacket())
            {
                p.writeH(3);
                p.writeH((short)room._roomId);
                p.writeH((short)room._channelId);
                p.writeC((byte)killer._slot);
                p.writeC((byte)objId);
                p.writeD(weaponId);
                p.writeTVector(killer.Position);
                p.writeC((byte)deaths.Count);
                for (int i = 0; i < deaths.Count; i++)
                {
                    DeathServerData ob = deaths[i];
                    p.writeC((byte)ob._player.WeaponClass);
                    p.writeC((byte)(((int)ob._deathType * 16) + ob._player._slot));
                    p.writeTVector(ob._player.Position);
                    p.writeC(0);
                }
                SendData(room, s, p.mstream.ToArray());
            }
        }
        public static void SendBombSync(Room room, Player pl, int type, int bombArea)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            using (SendPacket p = new SendPacket())
            {
                p.writeH(2);
                p.writeH((short)room._roomId);
                p.writeH((short)room._channelId);
                p.writeC((byte)type);
                p.writeC((byte)pl._slot);
                if (type == 0)
                {
                    p.writeC((byte)bombArea);
                    p.writeTVector(pl.Position);
                    room.BombPosition = pl.Position;
                }
                SendData(room, s, p.mstream.ToArray());
            }
        }
        public static void SendHitMarkerSync(Room room, Player pl, int deathType, int hitEnum, int damage)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            using (SendPacket p = new SendPacket())
            {
                p.writeH(4);
                p.writeH((short)room._roomId);
                p.writeH((short)room._channelId);
                p.writeC((byte)pl._slot);
                p.writeC((byte)deathType);
                p.writeC((byte)hitEnum);
                p.writeH((short)damage);
                SendData(room, s, p.mstream.ToArray());
            }
        }
        public static void SendSabotageSync(Room room, Player pl, int damage, int ultraSYNC)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            using (SendPacket p = new SendPacket())
            {
                p.writeH(5);
                p.writeH((short)room._roomId);
                p.writeH((short)room._channelId);
                p.writeC((byte)pl._slot);
                p.writeH((ushort)room._bar1);
                p.writeH((ushort)room._bar2);
                p.writeC((byte)ultraSYNC); //barnumber (1 = primeiro/2 = segundo)
                p.writeH((ushort)damage);
                SendData(room, s, p.mstream.ToArray());
            }
        }
        private static void SendData(Room room, Socket socket, byte[] data)
        {
            if (Config.sendInfoToServ)
                socket.SendTo(data, room.gs.Connection);
        }
    }
}