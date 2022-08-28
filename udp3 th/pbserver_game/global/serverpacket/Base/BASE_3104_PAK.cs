using Core.server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.global.serverpacket.Base
{
    public class BASE_3104_PAK : SendPacket
    {
        private int Error;
        private byte Year;

        public BASE_3104_PAK(int Error, byte Year)
        {
            this.Error = Error;
            this.Year = Year;
        }

        public override void write()
        {
            writeH(3104);
            writeD(Error);
            if (Error == 0)
            {
                writeC(Year);
            }
        }
    }
}
