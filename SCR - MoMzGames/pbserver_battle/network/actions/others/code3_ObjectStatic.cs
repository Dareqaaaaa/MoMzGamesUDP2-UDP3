namespace Battle.network.actions.others
{
    public class code3_ObjectStatic
    {
        public static byte[] ReadInfo(ReceivePacket p)
        {
            return p.readB(3);
        }
        public static Struct ReadInfo(ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                Life = p.readUH(),
                DestroyedBySlot = p.readC()
            };
            if (genLog)
                Logger.warning("[code3_ObjectStatic] Life: " + info.Life + "; DestroyedBy: " + info.DestroyedBySlot);
            return info;
        }
        public static void writeInfo(SendPacket s, ReceivePacket p)
        {
            s.writeB(ReadInfo(p));
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(p, genLog);
            s.writeH(info.Life);
            s.writeC(info.DestroyedBySlot);
            info = null;
        }
        public class Struct
        {
            public byte DestroyedBySlot;
            public ushort Life;
        }
    }
}