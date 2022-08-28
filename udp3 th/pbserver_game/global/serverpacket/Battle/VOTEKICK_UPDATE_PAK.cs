using Core.models.room;
using Core.server;

namespace Game.global.serverpacket
{
    public class VOTEKICK_UPDATE_PAK : SendPacket
    {
        private VoteKick _k;
        public VOTEKICK_UPDATE_PAK(VoteKick vote)
        {
            _k = vote;
        }

        public override void write()
        {
            writeH(3407);
            writeC((byte)_k.kikar);
            writeC((byte)_k.deixar);
        }
    }
}