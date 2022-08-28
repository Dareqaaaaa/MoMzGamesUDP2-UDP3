using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_COMMISSION_STAFF_PAK : SendPacket
    {
        private uint result;
        public CLAN_COMMISSION_STAFF_PAK(uint result)
        {
            this.result = result;
        }

        public override void write()
        {
            writeH(1341);
            writeD(result);
        }
    }
}