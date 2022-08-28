using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_MESSAGE_REQUEST_ACCEPT_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_MESSAGE_REQUEST_ACCEPT_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(1395);
            writeD(_erro);
        }
    }
}