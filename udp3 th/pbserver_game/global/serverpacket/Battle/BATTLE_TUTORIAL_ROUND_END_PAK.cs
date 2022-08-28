using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_TUTORIAL_ROUND_END_PAK : SendPacket
    {
        public BATTLE_TUTORIAL_ROUND_END_PAK()
        {
        }

        public override void write()
        {
            writeH(3395);
            writeC(3);
            writeD(110);
        }
    }
}