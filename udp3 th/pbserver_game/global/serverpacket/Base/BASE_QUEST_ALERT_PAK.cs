using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BASE_QUEST_ALERT_PAK : SendPacket
    {
        private Account p;
        private uint erro;
        private int type;
        public BASE_QUEST_ALERT_PAK(uint erro, int type, Account p)
        {
            this.erro = erro;
            this.type = type;
            this.p = p;
        }

        public override void write()
        {
            writeH(2602);
            writeD(erro); //Sem efeito - 0 || 1 - Efeito || 273 - finish?
            writeC((byte)type); //1 CardSetIdx?
            if ((erro & 1) == 1)
            {
                writeD(p._exp);
                writeD(p._gp);
                writeD(p.brooch);
                writeD(p.insignia);
                writeD(p.medal);
                writeD(p.blue_order);
                writeD(p._rank);
            }
        }
    }
}