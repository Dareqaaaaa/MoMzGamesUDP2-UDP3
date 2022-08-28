using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_PRIVILEGES_DEMOTE_PAK : SendPacket
    {
        public CLAN_PRIVILEGES_DEMOTE_PAK()
        {
        }

        public override void write()
        {
            writeH(1345);
        }
    }
}