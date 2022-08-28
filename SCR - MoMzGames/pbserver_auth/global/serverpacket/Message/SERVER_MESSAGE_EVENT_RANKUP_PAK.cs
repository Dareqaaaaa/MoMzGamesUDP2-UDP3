using Core.server;
using System;

namespace Auth.global.serverpacket
{
    public class SERVER_MESSAGE_EVENT_RANKUP_PAK : SendPacket
    {
        public SERVER_MESSAGE_EVENT_RANKUP_PAK()
        {
        }

        public override void write()
        {
            writeH(2616);
            //Interação bugada. Mensagem re-cria toda hora.
            //Bônus padrão no .exe: 50.000 GOLD.
        }
    }
}