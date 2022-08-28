namespace Battle.network.actions.others
{
    public class code6_ObjectAnim
    {
        public static byte[] ReadInfo(ReceivePacket p)
        {
            return p.readB(8);
        }
        public static Struct ReadInfo(ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _life = p.readUH(),
                _anim1 = p.readC(),
                _anim2 = p.readC(),
                _syncDate = p.readT()
            };
            if (genLog)
                Logger.warning("[code6_ObjectAnim] u: " + info._life + "; u2: " + info._anim1 + "; u3: " + info._anim2 + "; u4: " + info._syncDate);
            return info;
        }
        public static void writeInfo(SendPacket s, ReceivePacket p)
        {
            s.writeB(ReadInfo(p));
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(p, genLog);
            s.writeH(info._life);
            s.writeC(info._anim1);
            s.writeC(info._anim2);
            s.writeT(info._syncDate);
            info = null;
        }
        public class Struct
        {
            public float _syncDate;
            public byte _anim1, _anim2;
            public ushort _life;
        }
    }
}