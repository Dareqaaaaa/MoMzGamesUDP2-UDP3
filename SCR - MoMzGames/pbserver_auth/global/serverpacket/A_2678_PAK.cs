/*
 * Arquivo: A_2678_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/11/2016
 * Sintam inveja, não nos atinge
 */

using Core.server;

namespace Auth.global.serverpacket
{
    public class A_2678_PAK : SendPacket
    {
        public A_2678_PAK()
        {
        }

        public override void write()
        {
            writeH(2679);
            writeD(8);
            writeC(1);
            writeC(5);

            writeH(1); //versão jogo
            writeH(15); //versão jogo
            writeH(37); //versão jogo
            writeH(21); //versão jogo

            writeH(1012); //versão udp
            writeH(12); //versão udp

            writeC(5);
            writeS("Mar  2 2017", 11);
            writeD(0);
            writeS("11:10:23", 8);
            writeB(new byte[7]);
            writeS("DIST", 4);
            writeH(0);
        }
    }
}