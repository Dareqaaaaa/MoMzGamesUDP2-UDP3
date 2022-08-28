using Core.server;
using System;

namespace Game.global.serverpacket
{
    public class SHOP_ENTER_PAK : SendPacket
    {
        public SHOP_ENTER_PAK()
        {
        }

        public override void write()
        {
            writeH(2820);
            writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
        }
    }
}