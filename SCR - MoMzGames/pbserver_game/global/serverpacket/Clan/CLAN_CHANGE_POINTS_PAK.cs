using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CHANGE_POINTS_PAK : SendPacket
    {
        public CLAN_CHANGE_POINTS_PAK()
        {
        }

        public override void write()
        {
            writeH(1410);
        }
    }
}