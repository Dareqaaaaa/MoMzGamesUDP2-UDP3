using Core.server;

namespace Game.global.serverpacket
{
    public class AUTH_JACKPOT_NOTICE_PAK : SendPacket
    {
        private string _w;
        private int cupomId, _random;
        public AUTH_JACKPOT_NOTICE_PAK(string winner, int cupom, int rnd)
        {
            _w = winner;
            cupomId = cupom;
            _random = rnd;
        }

        public override void write()
        {
            writeH(557);
            writeC((byte)(_w.Length + 1));
            writeS(_w, _w.Length + 1);
            writeD(cupomId);
            writeC((byte)_random);
        }
    }
}