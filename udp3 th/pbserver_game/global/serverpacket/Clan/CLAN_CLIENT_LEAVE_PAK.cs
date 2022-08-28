using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CLIENT_LEAVE_PAK : SendPacket
    {
        public CLAN_CLIENT_LEAVE_PAK()
        {
        }

        public override void write()
        {
            writeH(1444);
        }
    }
}