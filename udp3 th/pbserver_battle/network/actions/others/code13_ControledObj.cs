using System;

namespace Battle.network.actions.others
{
    public class code13_ControledObj
    {
        public static Struct readSyncInfo(ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _unk = p.readB(9)
            }; //160 existe no mapa OUTPOST
            if (genLog)
                Logger.warning("[code13_ControledObj] " + BitConverter.ToString(info._unk));
            return info;
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            Struct info = readSyncInfo(p, genLog);
            s.writeB(info._unk);
            info = null;
        }
        public class Struct
        {
            public byte[] _unk;
        }
    }
}