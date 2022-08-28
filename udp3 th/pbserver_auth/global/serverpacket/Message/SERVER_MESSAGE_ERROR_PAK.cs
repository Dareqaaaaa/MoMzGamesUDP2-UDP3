/*
 * Arquivo: SERVER_MESSAGE_ERROR_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 27/11/2017
 * Sintam inveja, não nos atinge
 */

using Core.server;

namespace Auth.global.serverpacket
{
    public class SERVER_MESSAGE_ERROR_PAK : SendPacket
    {
        private uint _er;
        public SERVER_MESSAGE_ERROR_PAK(uint er)
        {
            _er = er;
        }

        public override void write()
        {
            writeH(2054);
            writeD(_er);
            /*
             * 0x800010AD STBL_IDX_EP_GAME_EXIT_HACKUSER
             * 0x800010AE STBL_IDX_EP_GAMEGUARD_ERROR (Ocorreu um problema no HackShield)
             * 0x800010AF STBL_IDX_EP_GAME_EXIT_ASSERT_E
             * 0x800010B0 STBL_IDX_EP_GAME_EXIT_ASSERT_E
             */
        }
    }
}