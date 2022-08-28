using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class CLAN_CREATE_REQUIREMENTS_REC : ReceiveGamePacket
    {
        public CLAN_CREATE_REQUIREMENTS_REC(GameClient client, byte[] data)
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
                if (_client != null)
                    _client.SendPacket(new CLAN_CREATE_REQUIREMENTS_PAK());
            }
            catch
            {
            }
        }
    }
}