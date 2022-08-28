using Core.server;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class BOX_MESSAGE_CHECK_READED_PAK : SendPacket
    {
        private List<int> msgs;
        public BOX_MESSAGE_CHECK_READED_PAK(List<int> msgs)
        {
            this.msgs = msgs;
        }

        public override void write()
        {
            writeH(423);
            writeC((byte)msgs.Count);
            for (int i = 0; i < msgs.Count; i++)
                writeD(msgs[i]);
        }
    }
}