using Core.managers;
using Core.server;
using System;

namespace Game.global.serverpacket
{
    public class CLAN_REQUEST_CONTEXT_PAK : SendPacket
    {
        private uint _erro;
        private int invites;
        public CLAN_REQUEST_CONTEXT_PAK(int clanId)
        {
            if (clanId > 0)
                invites = PlayerManager.getRequestCount(clanId);
            else
                _erro = 4294967295;
        }

        public override void write()
        {
            writeH(1321);
            writeD(_erro);
            if (_erro == 0)
            {
                writeC((byte)invites);
                writeC(13);
                writeC((byte)Math.Ceiling(invites / 13d));
                writeD(uint.Parse(DateTime.Now.ToString("MMddHHmmss")));
            }
        }
    }
}