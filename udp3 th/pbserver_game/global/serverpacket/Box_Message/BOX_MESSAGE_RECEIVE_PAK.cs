using Core.models.account;
using Core.server;

namespace Game.global.serverpacket
{
    public class BOX_MESSAGE_RECEIVE_PAK : SendPacket
    {
        private Message msg;
        public BOX_MESSAGE_RECEIVE_PAK(Message msg)
        {
            this.msg = msg;
        }

        public override void write()
        {
            writeH(427);
            writeD(msg.object_id);
            writeQ(msg.sender_id);
            writeC((byte)msg.type);
            writeC((byte)msg.state);
            writeC((byte)msg.DaysRemaining);
            writeD(msg.clanId);
            writeC((byte)(msg.sender_name.Length + 1));
            writeC((byte)(msg.type == 5 || msg.type == 4 && (int)msg.cB != 0 ? 0 : (msg.text.Length + 1)));
            writeS(msg.sender_name, msg.sender_name.Length + 1);
            if (msg.type == 5 || msg.type == 4)
            {
                if ((int)msg.cB >= 4 && (int)msg.cB <= 6)
                {
                    writeC((byte)(msg.text.Length + 1));
                    writeC((byte)msg.cB);
                    writeS(msg.text, msg.text.Length + 1);
                }
                else if ((int)msg.cB == 0)
                    writeS(msg.text, msg.text.Length + 1);
                else
                {
                    writeC(2);
                    writeH((short)msg.cB);
                }
            }
            else
                writeS(msg.text, msg.text.Length + 1);
        }
    }
}