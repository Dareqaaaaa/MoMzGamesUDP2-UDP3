using Core.server;

namespace Game.global.serverpacket
{
    public class LOBBY_CREATE_NICK_NAME_PAK : SendPacket
    {
        private uint _erro;
        public LOBBY_CREATE_NICK_NAME_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(3102);
            writeD(_erro);
        }
    }
}