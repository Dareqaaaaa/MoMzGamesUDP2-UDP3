namespace Battle.network.actions.others
{
    public class code9_StageInfoObjStatic
    {
        public static Struct readSyncInfo(ReceivePacket p, bool genLog)
        {
            Struct info = new Struct
            {
                _isDestroyed = p.readC() //0=Normal|1=Quebrado
            };
            if (genLog)
                Logger.warning("[code9_StageInfoObjStatic] u: " + info._isDestroyed);
            return info;
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            Struct info = readSyncInfo(p, genLog);
            s.writeC(info._isDestroyed);
            info = null;
        }
        public class Struct
        {
            public byte _isDestroyed;
        }
    }
}