using Battle.data.models;

namespace Battle.network.actions.user
{
    public class a2_unk
    {
        public static Struct ReadInfo(ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _unkV = p.readUH()
            }; //?
            if (genLog)
                Logger.warning("Slot " + ac._slot + " is moving the crosshair position: posV (" + info._unkV + ")");
            return info;
        }
        public static void writeInfo(SendPacket s, ActionModel ac, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(ac, p, genLog);
            s.writeH(info._unkV);
            info = null;
        }
        public class Struct
        {
            public ushort _unkV;
        }
    }
}