using Core.server;
using System;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_PARTY_CONTEXT_PAK : SendPacket
    {
        private int matchCount;
        public CLAN_WAR_PARTY_CONTEXT_PAK(int count)
        {
            matchCount = count;
        }

        public override void write()
        {
            writeH(1539);
            writeC((byte)matchCount);
            writeC(13);
            writeC((byte)Math.Ceiling(matchCount / 13d));
        }
    }
}