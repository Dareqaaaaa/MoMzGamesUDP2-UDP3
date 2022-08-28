using Core.server;

namespace Game.global.serverpacket
{
    public class VOTEKICK_CANCEL_VOTE_PAK : SendPacket
    {
        public VOTEKICK_CANCEL_VOTE_PAK()
        {
        }

        public override void write()
        {
            writeH(3405);
        }
    }
}