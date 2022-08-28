using Core.server;

namespace Game.global.serverpacket
{
    public class BASE_TITLE_USE_PAK : SendPacket
    {
        private uint _erro;
        public BASE_TITLE_USE_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(2622);
            writeD(_erro);
        }
    }
}