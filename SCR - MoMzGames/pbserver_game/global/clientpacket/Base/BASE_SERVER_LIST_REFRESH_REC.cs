using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class BASE_SERVER_LIST_REFRESH_REC : ReceiveGamePacket
    {
        public BASE_SERVER_LIST_REFRESH_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            if (_client != null)
                _client.SendPacket(new BASE_SERVER_LIST_REFRESH_PAK());
        }
    }
}