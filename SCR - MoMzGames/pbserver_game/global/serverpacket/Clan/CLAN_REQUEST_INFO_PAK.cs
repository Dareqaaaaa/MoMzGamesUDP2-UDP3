using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_REQUEST_INFO_PAK : SendPacket
    {
        private string text;
        private uint _erro;
        private Account p;
        public CLAN_REQUEST_INFO_PAK(long id, string txt)
        {
            text = txt;
            p = AccountManager.getAccount(id, 0);
            if (p == null || text == null)
                _erro = 0x80000000;
        }

        public override void write()
        {
            writeH(1325);
            writeD(_erro);
            if (_erro == 0)
            {
                writeQ(p.player_id);
                writeS(p.player_name, 33);
                writeC((byte)p._rank);
                writeD(p._statistic.kills_count);
                writeD(p._statistic.deaths_count);
                writeD(p._statistic.fights);
                writeD(p._statistic.fights_win);
                writeD(p._statistic.fights_lost);
                writeS(text, text.Length + 1);
            }
        }
    }
}