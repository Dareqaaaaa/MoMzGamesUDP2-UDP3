using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_REGIST_MERCENARY_PAK : SendPacket
    {
        private Match m;
        public CLAN_WAR_REGIST_MERCENARY_PAK(Match m)
        {
            this.m = m;
        }

        public override void write()
        {
            writeH(1552);
            writeH((short)m.getServerInfo());
            writeC((byte)m._state);
            writeC((byte)m.friendId);
            writeC((byte)m.formação);
            writeC((byte)m.getCountPlayers());
            writeD(m._leader);
            writeC(0);
            foreach (SLOT_MATCH s in m._slots)
            {
                Account p = m.getPlayerBySlot(s);
                if (p != null)
                {
                    writeC((byte)p._rank);
                    writeS(p.player_name, 33);
                    writeQ(s._playerId);
                    writeC((byte)s.state);
                }
                else writeB(new byte[43]);
            }
        }
    }
}