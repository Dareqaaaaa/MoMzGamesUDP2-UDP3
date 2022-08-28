using Core.server;

namespace Game.global.serverpacket
{
    public class SERVER_MESSAGE_ERROR_PAK : SendPacket
    {
        private uint _erro;
        public SERVER_MESSAGE_ERROR_PAK(uint err)
        {
            _erro = err;
        }

        public override void write()
        {
            writeH(2054);
            writeD(_erro);
            /*
             * 0x800010AD STBL_IDX_EP_GAME_EXIT_HACKUSER
             * 0x800010AE STBL_IDX_EP_GAMEGUARD_ERROR (Ocorreu um problema no HackShield)
             * 0x800010AF STBL_IDX_EP_GAME_EXIT_ASSERT_E
             * 0x800010B0 STBL_IDX_EP_GAME_EXIT_ASSERT_E
             */
        }
    }
}