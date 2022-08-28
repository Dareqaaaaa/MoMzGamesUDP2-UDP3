/*
 * Arquivo: A_EXIT_URL_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 14/10/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.enums.global;
using Core.server;

namespace Auth.global.serverpacket
{
    public class BASE_EXIT_URL_PAK : SendPacket
    {
        private int count;
        private string link;
        public BASE_EXIT_URL_PAK(string link)
        {
            this.count = (link.Length > 0 ? 1 : 0);
            this.link = link;
        }

        public override void write()
        {
            writeH(2694);
            writeC((byte)count);
            if (count > 0)
            {
                writeD(1);
                writeD((int)ClientLocale.Brazil);
                writeS(link, 256);
            } //Só considera o último link válido.
        }
    }
}