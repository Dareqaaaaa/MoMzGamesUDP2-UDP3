using Battle.data.enums;
using Battle.data.models;

namespace Battle.network.actions.user
{
    public class a8_MoveSync
    {
        public static Struct readSyncInfo(ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _spaceFlags = (CharaMoves)p.readC(),
                _objId = p.readUH()
            };
            if (genLog)
                Logger.warning("Slot " + ac._slot + " action 8: (" + info._spaceFlags + ";" + info._objId + ")");
            return info;
        }
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = readSyncInfo(ac, p, genLog);
            writeInfo(s, info);
            info = null;
        }
        public static void writeInfo(SendPacket s, Struct info)
        {
            s.writeC((byte)info._spaceFlags);
            s.writeH(info._objId);
        }
        public class Struct
        {
            public CharaMoves _spaceFlags;
            public ushort _objId;
        }
    }
}