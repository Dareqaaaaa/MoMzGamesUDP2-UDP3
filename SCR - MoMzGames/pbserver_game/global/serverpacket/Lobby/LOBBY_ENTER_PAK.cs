using Core.server;

namespace Game.global.serverpacket
{
    public class LOBBY_ENTER_PAK : SendPacket
    {
        public LOBBY_ENTER_PAK()
        {
        }

        public override void write()
        {
            writeH(3080);
            writeD(0);
        }
    }
}