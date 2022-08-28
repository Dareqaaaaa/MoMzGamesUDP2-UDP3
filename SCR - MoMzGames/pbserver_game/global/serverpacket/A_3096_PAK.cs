using Core.server;

namespace Game.global.serverpacket
{
    public class A_3096_PAK : SendPacket
    {
        private int XPEarned, GPEarned;
        public A_3096_PAK(int xp_earned, int gp_earned)
        {
            XPEarned = xp_earned;
            GPEarned = gp_earned;
        }

        public override void write()
        {
            writeH(3097);
            writeD(XPEarned);
            writeD(GPEarned);
        }
    }
}