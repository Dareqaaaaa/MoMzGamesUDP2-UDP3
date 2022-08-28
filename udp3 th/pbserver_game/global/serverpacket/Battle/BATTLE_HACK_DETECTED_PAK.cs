using Core.server;

namespace Game.global.serverpacket
{
    public class BATTLE_HACK_DETECTED_PAK : SendPacket
    {
        private int slotId;
        public BATTLE_HACK_DETECTED_PAK(int slot)
        {
            slotId = slot;
        }

        public override void write()
        {
            writeH(3413);
            writeC((byte)slotId); //Slot do hacker
            writeC(1); //?
            writeD(1); //?
        }
    }
}