using Core.server;

namespace Game.global.serverpacket
{
    public class BASE_SERVER_CHANGE_PAK : SendPacket
    {
        private int error;
        public BASE_SERVER_CHANGE_PAK(int error)
        {
            this.error = error;
        }

        public override void write()
        {
            writeH(2578);
            writeD(error);
            //error < 0 = STBL_IDX_EP_SERVER_FAIL_MOVE
        }
    }
}