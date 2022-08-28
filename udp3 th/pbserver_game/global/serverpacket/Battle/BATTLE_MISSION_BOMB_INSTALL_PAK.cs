using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_MISSION_BOMB_INSTALL_PAK : SendPacket
    {
        private int _slot;
        private float _x, _y, _z;
        private byte _zone;
        public BATTLE_MISSION_BOMB_INSTALL_PAK(int slot, byte zone, float x, float y, float z)
        {
            _zone = zone;
            _slot = slot;
            _x = x;
            _y = y;
            _z = z;
        }

        public override void write()
        {
            writeH(3357);
            writeD(_slot);
            writeC(_zone);
            writeH(42);
            writeT(_x);
            writeT(_y);
            writeT(_z);
        }
    }
}