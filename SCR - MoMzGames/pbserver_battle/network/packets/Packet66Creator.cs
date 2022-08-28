
namespace Battle.network.packets
{
    public class Packet66Creator
    {
        /// <summary>
        /// Gera um código do protocolo 66.
        /// </summary>
        /// <returns></returns>
        public static byte[] getCode66()
        {
            using (SendPacket s = new SendPacket())
            {
                s.writeC(66);
                s.writeC(0);
                s.writeT(0);
                s.writeC(0);
                s.writeH(13);
                s.writeD(0);
                return s.mstream.ToArray();
            }
        }
    }
}