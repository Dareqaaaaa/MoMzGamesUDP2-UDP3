using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_SENDPING_PAK : SendPacket
    {
        private byte[] _slots;
        public BATTLE_SENDPING_PAK(byte[] slots)
        {
            _slots = slots;
        }

        public override void write()
        {
            writeH(3345);
            writeB(_slots);
        }
    }
}