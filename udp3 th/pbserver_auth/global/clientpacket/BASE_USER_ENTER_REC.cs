using Core;
using Game.global.serverpacket;
using System;

namespace Auth.global.clientpacket
{
    public class BASE_USER_ENTER_REC : ReceiveLoginPacket
    {
        private string login;
        private byte[] _IPLocal;
        private long pId;
        public BASE_USER_ENTER_REC(LoginClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            login = readS(readC());
            pId = readQ();
            readC();
            _IPLocal = readB(4);
        }

        public override void run()
        {
            if (_client == null)
                return;
            try
            {
                Logger.warning("2579 received. [Now: " + DateTime.Now.ToString("yyMMddHHmmss") + "]");
                _client.SendPacket(new BASE_USER_ENTER_PAK(0x80000000));
            }
            catch
            {
                _client.Close(0, true);
            }
        }
    }
}