/*
 * Arquivo: BASE_USER_CONFIG_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 10/12/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account.players;
using Core.server;

namespace Auth.global.serverpacket
{
    public class BASE_USER_CONFIG_PAK : SendPacket
    {
        private int error;
        private PlayerConfig c;
        private bool isValid;
        public BASE_USER_CONFIG_PAK(int error, PlayerConfig config)
        {
            this.error = error;
            c = config;
            isValid = (c != null);
        }
        public BASE_USER_CONFIG_PAK(int error)
        {
            this.error = error;
        }
        public override void write()
        {
            writeH(2568);
            writeD(error);
            if (error < 0)
                return;
            writeC(!isValid); //1= Default | 0 = Customizable
            if (isValid)
            {
                writeH((short)c.blood);
                writeC((byte)c.sight);
                writeC((byte)c.hand);
                writeD(c.config);
                writeD(c.audio_enable);
                writeH(0);
                writeC((byte)c.audio1);
                writeC((byte)c.audio2);
                writeC((byte)c.fov);
                writeC(0);
                writeC((byte)c.sensibilidade);
                writeC((byte)c.mouse_invertido);
                writeH(0);
                writeC((byte)c.msgConvite);
                writeC((byte)c.chatSussurro);
                writeD(c.macro);
                writeB(new byte[] { 0, 57, 248, 16, 0 });
                writeB(c.keys);
                writeC((byte)(c.macro_1.Length + 1));
                writeS(c.macro_1, c.macro_1.Length + 1);
                writeC((byte)(c.macro_2.Length + 1));
                writeS(c.macro_2, c.macro_2.Length + 1);
                writeC((byte)(c.macro_3.Length + 1));
                writeS(c.macro_3, c.macro_3.Length + 1);
                writeC((byte)(c.macro_4.Length + 1));
                writeS(c.macro_4, c.macro_4.Length + 1);
                writeC((byte)(c.macro_5.Length + 1));
                writeS(c.macro_5, c.macro_5.Length + 1);
            }
        }
    }
}