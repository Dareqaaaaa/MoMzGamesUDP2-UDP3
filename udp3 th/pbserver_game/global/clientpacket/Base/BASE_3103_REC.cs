using Game.global.serverpacket.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.global.clientpacket.Base
{
    public class BASE_3103_REC : ReceiveGamePacket
    {
        private int Year;

        public BASE_3103_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            Year = readD();
        }

        public override void run()
        {
            int num = Year / 10000;
            int Time = int.Parse(DateTime.Now.ToString("yyyy"));
            int Result = Time - num;

            _client.SendPacket(new BASE_3104_PAK(0, (byte)Result));
        }
    }
}
