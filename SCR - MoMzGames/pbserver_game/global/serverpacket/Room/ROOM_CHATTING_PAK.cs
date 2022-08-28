using Core.server;

namespace Game.global.serverpacket
{
    public class ROOM_CHATTING_PAK : SendPacket
    {
        private string msg;
        private int type, slotId;
        private bool GMColor;
        public ROOM_CHATTING_PAK(int chat_type, int slotId, bool GM, string message)
        {
            type = chat_type;
            this.slotId = slotId;
            GMColor = GM;
            msg = message;
        }
        public override void write()
        {
            writeH(3851);
            writeH((short)type);
            writeD(slotId);
            writeC(GMColor);
            writeD(msg.Length + 1);
            writeS(msg, msg.Length + 1);
        }
    }
}