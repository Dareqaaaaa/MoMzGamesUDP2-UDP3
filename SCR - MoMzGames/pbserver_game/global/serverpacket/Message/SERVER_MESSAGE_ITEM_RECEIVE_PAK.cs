using Core.server;

namespace Game.global.serverpacket
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