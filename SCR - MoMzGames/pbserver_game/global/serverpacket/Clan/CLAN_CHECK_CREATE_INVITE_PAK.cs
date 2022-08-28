using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CHECK_CREATE_INVITE_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_CHECK_CREATE_INVITE_PAK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(1315);
            writeD(_erro);
        }
    }
}