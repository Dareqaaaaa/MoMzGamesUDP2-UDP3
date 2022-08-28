using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_CHAT_1390_PAK : SendPacket
    {
        private string sender, message;
        private int type, bantime;
        private bool isGM;
        public CLAN_CHAT_1390_PAK(Account p, string msg)
        {
            sender = p.player_name;
            message = msg;
            this.isGM = p.UseChatGM();
        }
        public CLAN_CHAT_1390_PAK(int type, int bantime)
        {
            this.type = type;
            this.bantime = bantime;
        }
        public override void write()
        {
            writeH(1391);
            writeC((byte)type);
            if (type == 0)
            {
                writeC((byte)(sender.Length + 1));
                writeS(sender, sender.Length + 1);
                writeC(isGM);
                writeC((byte)(message.Length + 1));
                writeS(message, message.Length + 1);
            }
            else
                writeD(bantime);
            /*
             * 1=STR_MESSAGE_BLOCK_ING
             * 2=STR_MESSAGE_BLOCK_START
             */
        }
    }
}