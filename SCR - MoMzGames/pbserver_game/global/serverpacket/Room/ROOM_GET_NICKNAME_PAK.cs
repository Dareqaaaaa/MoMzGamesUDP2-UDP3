using Core.server;

namespace Game.global.serverpacket
{
    public class ROOM_GET_NICKNAME_PAK : SendPacket
    {
        private int slotIdx, color;
        private string name;
        public ROOM_GET_NICKNAME_PAK(int slot, string name, int color)
        {
            slotIdx = slot;
            this.name = name;
            this.color = color;
        }

        public override void write()
        {
            writeH(3844);
            writeD(slotIdx);
            if (slotIdx >= 0)
            {
                writeS(name, 33);
                writeC((byte)color);
            }
        }
    }
}