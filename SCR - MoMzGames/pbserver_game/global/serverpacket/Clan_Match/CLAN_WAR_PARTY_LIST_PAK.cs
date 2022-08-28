using Core.server;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_PARTY_LIST_PAK : SendPacket
    {
        private List<Match> _c;
        private int _erro;
        public CLAN_WAR_PARTY_LIST_PAK(int erro, List<Match> c)
        {
            _erro = erro;
            _c = c;
        }

        public override void write()
        {
            writeH(1541);
            writeC((byte)(_erro == 0 ? _c.Count : _erro)); //Total de itens
            if (_erro > 0 || _c.Count == 0)
                return;
            writeC(1); //Página atual?
            writeC(0);
            writeC((byte)_c.Count); //Total de itens atual
            foreach (Match m in _c)
            {
                writeH((short)m._matchId);
                writeH((ushort)m.getServerInfo());
                writeH((ushort)m.getServerInfo());
                writeC((byte)m._state);
                writeC((byte)m.friendId);
                writeC((byte)m.formação);
                writeC((byte)m.getCountPlayers());
                writeC(0);//1
                writeD(m._leader);
                Account p = m.getLeader();
                if (p != null)
                {
                    writeC((byte)p._rank);
                    writeS(p.player_name, 33);
                    writeQ(p.player_id);
                    writeC((byte)m._slots[m._leader].state);
                }
                else
                    writeB(new byte[43]);
            }
        }
    }
}