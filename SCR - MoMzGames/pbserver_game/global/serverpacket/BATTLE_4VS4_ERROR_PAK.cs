using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_4VS4_ERROR_PAK : SendPacket
    {
        public BATTLE_4VS4_ERROR_PAK()
        {
        }

        public override void write()
        {
            writeH(3879);
        }
    }
}