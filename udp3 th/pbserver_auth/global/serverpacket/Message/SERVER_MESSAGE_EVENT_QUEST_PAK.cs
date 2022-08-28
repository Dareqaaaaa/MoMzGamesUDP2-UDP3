using Core.server;
using System;

namespace Auth.global.serverpacket
{
    public class SERVER_MESSAGE_EVENT_QUEST_PAK : SendPacket
    {
        public SERVER_MESSAGE_EVENT_QUEST_PAK()
        {
        }

        public override void write()
        {
            writeH(2061);
        }
    }
}