using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_MISSION_BOMB_UNINSTALL_PAK : SendPacket
    {
        private int _slot;
        public BATTLE_MISSION_BOMB_UNINSTALL_PAK(int slot)
        {
            _slot = slot;
        }

        public override void write()
        {
            writeH(3359);
            writeD(_slot);
        }
    }
}