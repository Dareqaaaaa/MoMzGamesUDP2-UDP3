using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BASE_QUEST_DELETE_CARD_SET_PAK : SendPacket
    {
        private uint erro;
        private Account p;
        public BASE_QUEST_DELETE_CARD_SET_PAK(uint erro, Account player)
        {
            this.erro = erro;
            this.p = player;
        }

        public override void write()
        {
            writeH(2608);
            writeD(erro);
            if (erro == 0)
            {
                writeC((byte)p._mission.actualMission);
                writeC((byte)p._mission.card1);
                writeC((byte)p._mission.card2);
                writeC((byte)p._mission.card3);
                writeC((byte)p._mission.card4);
                writeB(ComDiv.getCardFlags(p._mission.mission1, p._mission.list1));
                writeB(ComDiv.getCardFlags(p._mission.mission2, p._mission.list2));
                writeB(ComDiv.getCardFlags(p._mission.mission3, p._mission.list3));
                writeB(ComDiv.getCardFlags(p._mission.mission4, p._mission.list4));
                writeC((byte)p._mission.mission1);
                writeC((byte)p._mission.mission2);
                writeC((byte)p._mission.mission3);
                writeC((byte)p._mission.mission4);
            }
        }
    }
}