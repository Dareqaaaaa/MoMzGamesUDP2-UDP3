using Core.server;
using System;

namespace Game.global.serverpacket
{
    public class SHOP_LIST_PAK : SendPacket
    {
        public SHOP_LIST_PAK()
        {
        }

        public override void write()
        {
            writeH(2822);
            writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
        }
    }
}