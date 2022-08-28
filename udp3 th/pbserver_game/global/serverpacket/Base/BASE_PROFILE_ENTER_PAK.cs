using Core.server;

namespace Game.global.serverpacket
{
    public class BASE_PROFILE_ENTER_PAK : SendPacket
    {
        public BASE_PROFILE_ENTER_PAK()
        {
        }

        public override void write()
        {
            writeH(3863);
        }
    }
}