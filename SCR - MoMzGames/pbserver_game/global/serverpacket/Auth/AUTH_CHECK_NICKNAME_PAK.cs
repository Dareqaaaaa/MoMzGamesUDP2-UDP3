using Core.server;

namespace Game.global.serverpacket
{
    public class AUTH_CHECK_NICKNAME_PAK : SendPacket
    {
        private uint _erro;
        public AUTH_CHECK_NICKNAME_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(549);
            writeD(_erro);
        }
    }
}