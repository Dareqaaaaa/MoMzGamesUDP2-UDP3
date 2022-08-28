using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_CREATE_TEAM_PAK : SendPacket
    {
        private uint _erro;
        private Match m;
        public CLAN_WAR_CREATE_TEAM_PAK(uint erro, Match mt = null)
        {
            _erro = erro;
            m = mt;
        }
        public override void write()
        {
            writeH(1547);
            writeD(_erro);
            if (_erro == 0)
            {
                writeH((short)m._matchId);
                writeH((short)m.getServerInfo());
                writeH((short)m.getServerInfo());
                writeC((byte)m._state);
                writeC((byte)m.friendId);
                writeC((byte)m.formação);
                writeC((byte)m.getCountPlayers());
                writeD(m._leader);
                writeC(0);
            }
        }
    }
}