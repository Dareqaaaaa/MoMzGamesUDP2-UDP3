using Core.server;

namespace Game.global.serverpacket
{
    public class A_3329_PAK : SendPacket
    {
        public A_3329_PAK()
        {
        }

        public override void write()
        {
            writeH(3330);
            writeD(0);
        }
    }
}