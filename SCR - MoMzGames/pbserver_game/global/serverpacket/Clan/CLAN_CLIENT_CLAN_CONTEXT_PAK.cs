using Core.server;
using System;

namespace Game.global.serverpacket
{
    public class CLAN_CLIENT_CLAN_CONTEXT_PAK : SendPacket
    {
        private int clansCount;
        public CLAN_CLIENT_CLAN_CONTEXT_PAK(int count)
        {
            clansCount = count;
        }

        public override void write()
        {
            writeH(1452);
            writeD(clansCount);
            writeC(170);
            writeH((ushort)Math.Ceiling(clansCount / 170d));
            writeD(uint.Parse(DateTime.Now.ToString("MMddHHmmss")));
        }
    }
}