using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_MATCH_PROPOSE_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_WAR_MATCH_PROPOSE_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(1554);
            writeD(_erro);
        }
    }
}