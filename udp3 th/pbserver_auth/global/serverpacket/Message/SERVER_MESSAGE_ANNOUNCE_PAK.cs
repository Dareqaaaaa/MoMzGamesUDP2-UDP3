using Core.models.enums;
using Core.server;

namespace Auth.global.serverpacket
{
    public class SERVER_MESSAGE_ANNOUNCE_PAK : SendPacket
    {
        private string _message;
        /// <summary>
        /// Envia uma mensagem global ao jogador. (GM)
        /// </summary>
        /// <param name="msg"></param>
        public SERVER_MESSAGE_ANNOUNCE_PAK(string msg)
        {
            _message = msg;
        }

        public override void write()
        {
            writeH(2055);
            writeD(2); //Tipo da notícia [NOTICE_TYPE_NORMAL - 1 || NOTICE_TYPE_EMERGENCY - 2]
            writeH((ushort)_message.Length);
            writeS(_message);
        }
    }
}