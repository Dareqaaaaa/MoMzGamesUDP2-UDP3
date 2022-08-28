using Core;
using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CREATE_REQUIREMENTS_PAK : SendPacket
    {
        public CLAN_CREATE_REQUIREMENTS_PAK()
        {
        }

        public override void write()
        {
            writeH(1417);
            writeC((byte)ConfigGS.minCreateRank);
            writeD(ConfigGS.minCreateGold);
        }
    }
}