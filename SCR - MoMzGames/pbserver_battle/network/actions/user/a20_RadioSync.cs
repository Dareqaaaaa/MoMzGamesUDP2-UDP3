using Battle.data.models;

namespace Battle.network.actions.user
{
    public class a20_RadioSync
    {
        public static Struct readSyncInfo(ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _radioId = p.readC(),
                _areaId = p.readC()
            };
            if (genLog)
                Logger.warning("Slot " + ac._slot + " released a radio chat: radId, areaId [" + info._radioId + ";" + info._areaId + "]");
            return info;
        }
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = readSyncInfo(ac, p, genLog);
            s.writeC(info._radioId);
            s.writeC(info._areaId);
            info = null;
        }
        public class Struct
        {
            public byte _radioId, _areaId;
        }
    }
}