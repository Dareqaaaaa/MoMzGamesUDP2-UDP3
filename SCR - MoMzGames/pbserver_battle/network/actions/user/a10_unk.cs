using Battle.data.models;

namespace Battle.network.actions.user
{
    public class a10_unk
    {
        public static Struct readSyncInfo(ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _unkV = p.readC(),
                _unkV2 = p.readC()
            };
            if (genLog)
                Logger.warning("Slot " + ac._slot + " action 16: unk (" + info._unkV + ";" + info._unkV2 + ")");
            return info;
        }
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = readSyncInfo(ac, p, genLog);
            s.writeC(info._unkV);
            s.writeC(info._unkV2);
            info = null;
        }
        public class Struct
        {
            public byte _unkV, _unkV2;
        }
    }
}
