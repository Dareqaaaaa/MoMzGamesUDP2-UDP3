using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_REQUEST_DENIAL_PAK : SendPacket
    {
        private int result;
        public CLAN_REQUEST_DENIAL_PAK(int result)
        {
            this.result = result;
        }

        public override void write()
        {
            writeH(1330);
            writeD(result);
        }
    }
}