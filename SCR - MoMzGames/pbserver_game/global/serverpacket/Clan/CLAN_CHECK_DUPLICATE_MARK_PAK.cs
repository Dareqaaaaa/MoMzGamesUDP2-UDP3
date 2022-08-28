using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CHECK_DUPLICATE_MARK_PAK : SendPacket
    {
        private uint _erro;
        public CLAN_CHECK_DUPLICATE_MARK_PAK(uint er)
        {
            _erro = er;
        }

        public override void write()
        {
            writeH(1361);
            writeD(_erro);
        }
    }
}