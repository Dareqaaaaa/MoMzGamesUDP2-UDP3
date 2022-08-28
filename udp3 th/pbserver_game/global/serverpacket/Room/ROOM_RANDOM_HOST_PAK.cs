using Core.server;

namespace Game.global.serverpacket
{
    public class ROOM_RANDOM_HOST_PAK : SendPacket
    {
        private uint _slot;
        public ROOM_RANDOM_HOST_PAK(uint slot)
        {
            _slot = slot;
        }
        public ROOM_RANDOM_HOST_PAK(int slot)
        {
            _slot = (uint)slot;
        }
        public override void write()
        {
            writeH(3869);
            writeD(_slot);
        }
    }
}