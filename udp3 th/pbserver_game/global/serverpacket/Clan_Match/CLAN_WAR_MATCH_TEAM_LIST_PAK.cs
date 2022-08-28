using Core.server;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_MATCH_TEAM_LIST_PAK : SendPacket
    {
        private List<Match> matchs;
        private int myMatchIdx, _page, MatchCount;
        public CLAN_WAR_MATCH_TEAM_LIST_PAK(int page, List<Match> matchs, int matchId)
        {
            _page = page;
            myMatchIdx = matchId;
            MatchCount = (matchs.Count - 1);
            this.matchs = matchs;
        }

        public override void write()
        {
            writeH(1545);
            writeH((ushort)MatchCount);//Quantidade de clãs na lista
            if (MatchCount == 0)
                return;
            writeH(1);
            writeH(0);
            writeC((byte)MatchCount); //Quantidade de itens da lista a ser lida
            for (int i = 0; i < matchs.Count; i++)
            {
                Match m = matchs[i];
                if (m._matchId == myMatchIdx)
                    continue;
                writeH((short)m._matchId);
                writeH((short)m.getServerInfo());
                writeH((short)m.getServerInfo());
                writeC((byte)m._state);
                writeC((byte)m.friendId);
                writeC((byte)m.formação);
                writeC((byte)m.getCountPlayers());
                writeD(m._leader);
                writeC(0);
                writeD(m.clan._id);
                writeC((byte)m.clan._rank);
                writeD(m.clan._logo);
                writeS(m.clan._name, 17);
                writeT(m.clan._pontos);
                writeC((byte)m.clan._name_color);
                Account p = m.getLeader();
                if (p != null)
                {
                    writeC((byte)p._rank);
                    writeS(p.player_name, 33);
                    writeQ(p.player_id);
                    writeC((byte)m._slots[m._leader].state);
                }
                else writeB(new byte[43]);
            }
        }
    }
}