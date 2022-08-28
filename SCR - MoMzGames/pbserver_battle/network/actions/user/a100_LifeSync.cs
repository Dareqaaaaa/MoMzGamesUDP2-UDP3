namespace Battle.network.actions.user
{
    public class a100_LifeSync
    {
        public static ushort ReadInfo(ReceivePacket p, bool genLog)
        {
            return p.readUH();
        }
        public static void ReadInfo(ReceivePacket p)
        {
            p.Advance(2);
        }
        public static void writeInfo(SendPacket s, ReceivePacket p, bool genLog)
        {
            s.writeH(ReadInfo(p, genLog));
        }
    }
}