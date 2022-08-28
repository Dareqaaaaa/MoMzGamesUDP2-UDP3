using Core;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BASE_USER_EXIT_REC : ReceiveGamePacket
    {
        public BASE_USER_EXIT_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }
        public override void read()
        {
        }
        public override void run()
        {
            try
            {
                _client.SendPacket(new BASE_USER_EXIT_PAK());
                _client.Close(1000);
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
                _client.Close(0);
            }
        }
    }
}