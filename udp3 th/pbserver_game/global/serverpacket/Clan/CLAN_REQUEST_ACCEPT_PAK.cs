using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_REQUEST_ACCEPT_PAK : SendPacket
    {
        private uint result;
        public CLAN_REQUEST_ACCEPT_PAK(uint result)
        {
            this.result = result;
        }

        public override void write()
        {
            writeH(1327);
            writeD(result);
        }
    }
}