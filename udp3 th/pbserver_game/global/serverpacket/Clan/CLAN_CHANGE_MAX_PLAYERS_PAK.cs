using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CHANGE_MAX_PLAYERS_PAK : SendPacket
    {
        private int _max;
        public CLAN_CHANGE_MAX_PLAYERS_PAK(int max)
        {
            _max = max;
        }

        public override void write()
        {
            writeH(1377);
            writeC((byte)_max);
        }
    }
}