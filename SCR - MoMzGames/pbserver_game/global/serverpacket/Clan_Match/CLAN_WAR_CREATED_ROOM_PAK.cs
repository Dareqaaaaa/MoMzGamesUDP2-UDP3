using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_CREATED_ROOM_PAK : SendPacket
    {
        public Match _mt;
        public CLAN_WAR_CREATED_ROOM_PAK(Match match)
        {
            _mt = match;
        }

        public override void write()
        {
            writeH(1564);
            writeH((short)_mt._matchId);
            writeD(_mt.getServerInfo());
            writeH((short)_mt.getServerInfo());
            writeC(10);
        }
    }
}