/*
 * Arquivo: BASE_SERVER_ENTER_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/11/2016
 * Sintam inveja, não nos atinge
 */

using Core.server;

namespace Auth.global.serverpacket
{
    public class BASE_SERVER_CHANGE_PAK : SendPacket
    {
        private int erro;
        public BASE_SERVER_CHANGE_PAK(int erro)
        {
            this.erro = erro;
        }

        public override void write()
        {
            writeH(2578);
            writeD(erro);
        }
    }
}