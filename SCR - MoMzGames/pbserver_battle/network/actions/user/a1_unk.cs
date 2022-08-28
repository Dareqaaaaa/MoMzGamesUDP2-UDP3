using Battle.data.enums;
using Battle.data.models;
using Battle.network;

namespace Battle.network.actions.user
{
    public class a1_unk
    {
        public static Struct ReadInfo(ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _unkV = p.readC(), //?
                _unkV2 = p.readC(), //?
                _unkV3 = p.readC(), //?
                _unkV4 = p.readC()
            }; //?
            if (genLog)
            {
                //Logger.warning("u1: " + (BasicFlags)info._unkV);
                //Logger.warning("u2: " + (BasicFlags)info._unkV2);
                //Logger.warning("u3: " + (BasicFlags)info._unkV3);
                //Logger.warning("u4: " + (BasicFlags)info._unkV4);
                //Logger.warning("u: " + info._unkV + ";" + info._unkV2 + ";" + info._unkV3 + ";" + info._unkV4);
            }
            return info;
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(p, genLog);
            writeInfo(s, info);
            info = null;
        }
        public static void writeInfo(SendPacket s, Struct info)
        {
            s.writeC(info._unkV);
            s.writeC(info._unkV2);
            s.writeC(info._unkV3);
            s.writeC(info._unkV4);
        }
        public class Struct
        {
            public byte _unkV, _unkV2, _unkV3, _unkV4;
        }
    }
}