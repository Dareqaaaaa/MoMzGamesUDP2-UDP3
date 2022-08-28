using Core;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BASE_SERVER_CHANGE_REC : ReceiveGamePacket
    {
        public BASE_SERVER_CHANGE_REC(GameClient client, byte[] data)
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
                _client.SendPacket(new BASE_SERVER_CHANGE_PAK(0));
                _client.Close(0);
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }
    }
}