using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_RESPAWN_FOR_AI_PAK : SendPacket
    {
        private int _slot;
        public BATTLE_RESPAWN_FOR_AI_PAK(int slot)
        {
            _slot = slot;
        }

        public override void write()
        {
            writeH(3379);
            writeD(_slot);
        }
    }
}