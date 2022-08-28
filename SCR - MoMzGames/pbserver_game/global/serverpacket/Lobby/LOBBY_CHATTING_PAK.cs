using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class LOBBY_CHATTING_PAK : SendPacket
    {
        private string sender, msg;
        private uint sessionId;
        private int nameColor;
        private bool GMColor;
        public LOBBY_CHATTING_PAK(Account player, string message, bool GMCmd = false)
        {
            if (!GMCmd)
            {
                nameColor = player.name_color;
                GMColor = player.UseChatGM();
            }
            else
                GMColor = true;
            sender = player.player_name;
            sessionId = player.getSessionId();
            msg = message;
        }
        public LOBBY_CHATTING_PAK(string snd, uint session, int name_color, bool chatGm, string message)
        {
            sender = snd;
            sessionId = session;
            nameColor = name_color;
            GMColor = chatGm;
            msg = message;
        }
        public override void write()
        {
            writeH(3093);
            writeD(sessionId);
            writeC((byte)(sender.Length + 1));
            writeS(sender, sender.Length + 1);
            writeC((byte)nameColor);
            writeC(GMColor);
            writeH((ushort)(msg.Length + 1));
            writeS(msg, msg.Length + 1);
        }
    }
}