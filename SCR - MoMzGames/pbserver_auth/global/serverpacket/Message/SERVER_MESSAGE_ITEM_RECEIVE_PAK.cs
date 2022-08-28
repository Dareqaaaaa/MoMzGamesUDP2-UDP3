/*
 * Arquivo: SERVER_MESSAGE_ITEM_RECEIVE_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/11/2016
 * Sintam inveja, não nos atinge
 */

using Core.server;

namespace Auth.global.serverpacket
{
    public class SERVER_MESSAGE_ITEM_RECEIVE_PAK : SendPacket
    {
        private uint _er;
        public SERVER_MESSAGE_ITEM_RECEIVE_PAK(uint er)
        {
            _er = er;
        }

        public override void write()
        {
            writeH(2692);
            writeD(_er);
        }
    }
}