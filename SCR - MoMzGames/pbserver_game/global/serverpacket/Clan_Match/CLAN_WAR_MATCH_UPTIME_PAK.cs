using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_MATCH_UPTIME_PAK : SendPacket
    {
        private int _f;
        private uint _erro;
        public CLAN_WAR_MATCH_UPTIME_PAK(uint erro, int formacao = 0)
        {
            _erro = erro;
            _f = formacao;
        }

        public override void write()
        {
            writeH(1572);
            writeD(_erro);
            if (_erro == 0)
                writeC((byte)_f);
        }
    }
}