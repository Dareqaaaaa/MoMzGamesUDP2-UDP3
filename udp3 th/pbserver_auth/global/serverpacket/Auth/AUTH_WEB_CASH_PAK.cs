/*
 * Arquivo: BASE_USER_CASHING_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/11/2016
 * Sintam inveja, não nos atinge
 */

using Core.server;

namespace Auth.global.serverpacket
{
    public class AUTH_WEB_CASH_PAK : SendPacket
    {
        private int erro, gold, cash;
        public AUTH_WEB_CASH_PAK(int erro, int gold = 0, int cash = 0)
        {
            this.erro = erro;
            this.gold = gold;
            this.cash = cash;
        }

        public override void write()
        {
            writeH(545);
            writeD(erro);
            if (erro >= 0)
            {
                writeD(gold);
                writeD(cash);
            }
        }
    }
}