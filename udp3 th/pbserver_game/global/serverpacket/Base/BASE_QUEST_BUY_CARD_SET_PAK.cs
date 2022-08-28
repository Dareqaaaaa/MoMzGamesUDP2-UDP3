using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BASE_QUEST_BUY_CARD_SET_PAK : SendPacket
    {
        private Account player;
        private uint _erro;
        public BASE_QUEST_BUY_CARD_SET_PAK(uint erro, Account p)
        {
            _erro = erro;
            player = p;
        }

        public override void write()
        {
            writeH(2606);
            writeD(_erro);
            if (_erro == 0)
            {
                writeD(player._gp);
                writeC((byte)player._mission.actualMission);
                writeC((byte)player._mission.actualMission);
                writeC((byte)player._mission.card1);
                writeC((byte)player._mission.card2);
                writeC((byte)player._mission.card3);
                writeC((byte)player._mission.card4);
                writeB(ComDiv.getCardFlags(player._mission.mission1, player._mission.list1));
                writeB(ComDiv.getCardFlags(player._mission.mission2, player._mission.list2));
                writeB(ComDiv.getCardFlags(player._mission.mission3, player._mission.list3));
                writeB(ComDiv.getCardFlags(player._mission.mission4, player._mission.list4));
                writeC((byte)player._mission.mission1);
                writeC((byte)player._mission.mission2);
                writeC((byte)player._mission.mission3);
                writeC((byte)player._mission.mission4);
            }
        }
    }
}