using Core.server;

namespace Game.global.serverpacket
{
    public class BASE_HACK_PAK : SendPacket
    {
        private byte[] _u;
        public BASE_HACK_PAK(byte[] u)
        {
            _u = u;
        }

        public override void write()
        {
            writeH(2583);
            writeB(new byte[512]);
        }
    }
}