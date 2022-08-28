using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_PRIVILEGES_KICK_PAK : SendPacket
    {
        public CLAN_PRIVILEGES_KICK_PAK()
        {
        }

        public override void write()
        {
            writeH(1336);
        }
    }
}