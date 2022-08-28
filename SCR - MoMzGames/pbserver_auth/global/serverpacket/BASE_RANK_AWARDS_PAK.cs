/*
 * Arquivo: BASE_RANK_AWARDS_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 12/01/2017
 * Sintam inveja, não nos atinge
 */

using Core.server;

namespace Auth.global.serverpacket
{
    public class BASE_RANK_AWARDS_PAK : SendPacket
    {
        public BASE_RANK_AWARDS_PAK()
        {
        }

        public override void write()
        {
            writeH(2667);
            for (int i = 0; i < 50; i++)
            {
                writeC((byte)(i + 1));
                writeD(0);
                writeD(0);
                writeD(0);
                writeD(0);
            }
        }
    }
}