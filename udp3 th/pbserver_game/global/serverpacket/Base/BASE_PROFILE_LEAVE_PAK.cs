using Core.server;

namespace Game.global.serverpacket
{
    public class BASE_PROFILE_LEAVE_PAK : SendPacket
    {
        public BASE_PROFILE_LEAVE_PAK()
        {
        }

        public override void write()
        {
            writeH(3865);
        }
    }
}