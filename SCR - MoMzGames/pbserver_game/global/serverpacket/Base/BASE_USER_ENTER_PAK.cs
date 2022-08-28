using Core.server;

namespace Game.global.serverpacket
{
    public class BASE_USER_ENTER_PAK : SendPacket
    {
        private uint _erro;
        public BASE_USER_ENTER_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(2580);
            writeD(_erro);
        }
    }
}