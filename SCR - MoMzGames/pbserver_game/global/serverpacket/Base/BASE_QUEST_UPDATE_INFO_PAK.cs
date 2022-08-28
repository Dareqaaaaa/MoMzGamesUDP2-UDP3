using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BASE_QUEST_UPDATE_INFO_PAK : SendPacket
    {
        private Account p;
        public BASE_QUEST_UPDATE_INFO_PAK(Account p)
        {
            this.p = p;
        }

        public override void write()
        {
            writeH(2604);
            if (p != null)
            {
                writeQ(p.player_id);
                writeD(p.brooch);
                writeD(p.insignia);
                writeD(p.medal);
                writeD(p.blue_order);
            }
            else
                writeB(new byte[24]);
        }
    }
}