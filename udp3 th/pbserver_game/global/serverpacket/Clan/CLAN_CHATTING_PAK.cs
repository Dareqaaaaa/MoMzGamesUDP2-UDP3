using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_CHATTING_PAK : SendPacket
    {
        private string text;
        private Account p;
        private int type, bantime;
        public CLAN_CHATTING_PAK(string text, Account player)
        {
            this.text = text;
            p = player;
        }
        public CLAN_CHATTING_PAK(int type, int bantime)
        {
            this.type = type;
            this.bantime = bantime;
        }
        public override void write()
        {
            writeH(1359);
            writeC((byte)type);
            if (type == 0)
            {
                writeC((byte)(p.player_name.Length + 1));
                writeS(p.player_name, p.player_name.Length + 1);
                writeC(p.UseChatGM());
                writeC((byte)(text.Length + 1));
                writeS(text, text.Length + 1);
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