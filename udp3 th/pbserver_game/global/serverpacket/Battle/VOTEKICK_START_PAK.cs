using Core.models.room;
using Core.server;

namespace Game.global.serverpacket
{
    public class VOTEKICK_START_PAK : SendPacket
    {
        private VoteKick vote;
        public VOTEKICK_START_PAK(VoteKick vote)
        {
            this.vote = vote;
        }

        public override void write()
        {
            writeH(3399);
            writeC((byte)vote.creatorIdx);
            writeC((byte)vote.victimIdx);
            writeC((byte)vote.motive);
        }
    }
}