using Battle.data.models;
using Battle.data.sync;

namespace Battle.network.actions.user
{
    public class a100000_PassPortal
    {
        public static Struct ReadInfo(ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _portal = p.readUH()
            };
            if (genLog)
                Logger.warning("Slot " + ac._slot + " passed on the portal [" + info._portal + "]");
            return info;
        }
        public static void ReadInfo(ReceivePacket p)
        {
            p.Advance(2);
        }
        public static void SendPassSync(Room room, Player p, Struct info)
        {
            Battle_SyncNet.SendPortalPass(room, p, info._portal);
        }
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(ac, p, genLog);
            writeInfo(s, info);
            info = null;
        }
        public static void writeInfo(SendPacket s, Struct info)
        {
            s.writeH(info._portal);
        }
        public class Struct
        {
            public ushort _portal;
        }
    }
}