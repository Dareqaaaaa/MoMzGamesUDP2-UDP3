using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CHECK_DUPLICATE_NAME_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_CHECK_DUPLICATE_NAME_PAK(uint erro)
        {
            _erro = erro;
        }
        public override void write()
        {
            writeH(1448);
            writeD(_erro);
        }
    }
}