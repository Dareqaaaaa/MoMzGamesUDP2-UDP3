using Core.server;

namespace Game.global.serverpacket
{
    public class FRIEND_ACCEPT_PAK : SendPacket
    {
        private uint _erro;
        public FRIEND_ACCEPT_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(281);
            writeD(_erro);
        }
    }
}