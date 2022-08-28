using Core.server;

namespace Game.global.serverpacket
{
    public class BASE_SERVER_PASSW_PAK : SendPacket
    {
        private uint _erro;
        public BASE_SERVER_PASSW_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(2645);
            writeD(_erro);
        }
    }
}