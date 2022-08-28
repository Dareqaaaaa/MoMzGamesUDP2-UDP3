using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BATTLE_LEAVEP2PSERVER_PAK : SendPacket
    {
        private Account p;
        private int type;
        public BATTLE_LEAVEP2PSERVER_PAK(Account p, int type)
        {
            this.p = p;
            this.type = type;
        }

        public override void write()
        {
            if (p == null)
                return;
            writeH(3385);
            writeD(p._slotId);
            writeC((byte)type);
            writeD(p._exp);
            writeD(p._rank);
            writeD(p._gp);
            writeD(p._statistic.escapes);
            writeD(p._statistic.escapes);
        }
    }
}