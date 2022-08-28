using Core.server;

namespace Game.global.serverpacket
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