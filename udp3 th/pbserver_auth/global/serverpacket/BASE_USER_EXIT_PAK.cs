/*
 * Arquivo: BASE_USER_EXIT_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/11/2016
 * Sintam inveja, não nos atinge
 */

using Core.server;

namespace Auth.global.serverpacket
{
    public class BASE_USER_EXIT_PAK : SendPacket
    {
        public BASE_USER_EXIT_PAK()
        {
        }

        public override void write()
        {
            writeH(2655);
        }
    }
}