using Core.server;

namespace Game.global.serverpacket
{
    public class BASE_RANK_UP_PAK : SendPacket
    {
        private int _rank, _allExp;
        public BASE_RANK_UP_PAK(int rank, int allExp)
        {
            _rank = rank;
            _allExp = allExp;
        }

        public override void write()
        {
            writeH(2614);
            writeD(_rank);
            writeD(_rank);
            writeD(_allExp); //EXP
        }
    }
}