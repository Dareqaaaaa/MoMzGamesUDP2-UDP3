using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CLOSE_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_CLOSE_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(1313);
            writeD(_erro);
        }
    }
}