namespace Battle.network.actions.others
{
    public class code12_StageObjAnim
    {
        public static byte[] ReadInfo(ReceivePacket p)
        {
            return p.readB(9);
        }
        public static Struct ReadInfo(ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _unk = p.readC(),
                _life = p.readUH(),
                _syncDate = p.readT(),
                _anim1 = p.readC(),
                _anim2 = p.readC()
            };
            if (genLog)
                Logger.warning("[code12_StageObjAnim] " + info._unk + ";" + info._life + ";" + info._syncDate + ";" + info._anim1 + ";" + info._anim2);
            return info;
        }
        public static void writeInfo(SendPacket s, ReceivePacket p)
        {
            s.writeB(ReadInfo(p));
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            Struct info = ReadInfo(p, genLog);
            s.writeC(info._unk);
            s.writeH(info._life);
            s.writeT(info._syncDate);
            s.writeC(info._anim1);
            s.writeC(info._anim2);
            info = null;
        }
        public class Struct
        {
            public byte _unk, _anim1, _anim2;
            public float _syncDate;
            public ushort _life;
        }
    }
}