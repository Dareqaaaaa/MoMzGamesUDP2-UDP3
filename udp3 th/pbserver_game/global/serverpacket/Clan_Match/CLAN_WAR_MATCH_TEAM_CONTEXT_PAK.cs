using Core.server;
using System;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_MATCH_TEAM_CONTEXT_PAK : SendPacket
    {
        private int count;
        public CLAN_WAR_MATCH_TEAM_CONTEXT_PAK(int count)
        {
            this.count = count;
        }

        public override void write()
        {
            writeH(1543);
            writeH((short)count);
            writeC(13);
            writeH((short)Math.Ceiling(count / 13d));
        }
    }
}