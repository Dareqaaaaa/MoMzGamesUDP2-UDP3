using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BASE_QUEST_GET_INFO_PAK : SendPacket
    {
        private Account player;
        public BASE_QUEST_GET_INFO_PAK(Account player)
        {
            this.player = player;
        }

        public override void write()
        {
            writeH(2596);
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