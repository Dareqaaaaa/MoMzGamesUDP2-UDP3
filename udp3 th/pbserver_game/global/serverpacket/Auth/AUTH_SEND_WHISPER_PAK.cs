using Core.server;

namespace Game.global.serverpacket
{
    public class AUTH_SEND_WHISPER_PAK : SendPacket
    {
        private string name, msg;
        private uint erro;
        private int type, bantime;
        public AUTH_SEND_WHISPER_PAK(string name, string msg, uint erro)
        {
            this.name = name;
            this.msg = msg;
            this.erro = erro;
        }
        public AUTH_SEND_WHISPER_PAK(int type, int bantime)
        {
            this.type = type;
            this.bantime = bantime;
        }
        public override void write()
        {
            writeH(291);
            writeC((byte)type);
            if (type == 0)
            {
                writeD(erro);
                writeS(name, 33);
                if (erro == 0)
                {
                    writeH((ushort)(msg.Length + 1));
                    writeS(msg, msg.Length + 1);
                }
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