using Core.server;

namespace Game.global.serverpacket
{
    public class CLAN_CHANGE_NAME_COLOR_PAK : SendPacket
    {
        private byte color;
        public CLAN_CHANGE_NAME_COLOR_PAK(byte color)
        {
            this.color = color;
        }

        public override void write()
        {
            writeH(1406);
            writeC(color);
        }
    }
}