using Core.server;
using System;

namespace Game.global.serverpacket
{
    public class INVENTORY_ENTER_PAK : SendPacket
    {
        public INVENTORY_ENTER_PAK()
        {
        }

        public override void write()
        {
            writeH(3586);
            writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
        }
    }
}