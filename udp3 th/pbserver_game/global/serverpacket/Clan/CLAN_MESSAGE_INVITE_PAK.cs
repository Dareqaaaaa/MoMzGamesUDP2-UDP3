using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_MESSAGE_INVITE_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_MESSAGE_INVITE_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(1393);
            writeD(_erro);
        }
    }
}