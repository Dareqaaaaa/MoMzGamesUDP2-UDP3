using Game.data.managers;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
    public class CLAN_CHECK_DUPLICATE_NAME_REC : ReceiveGamePacket
    {
        private string clanName;
        public CLAN_CHECK_DUPLICATE_NAME_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            clanName = readS(readC());
        }

        public override void run()
        {
            if (_client == null || _client._player == null)
                return;
            try
            {
                _client.SendPacket(new CLAN_CHECK_DUPLICATE_NAME_PAK(!ClanManager.isClanNameExist(clanName) ? 0 : 0x80000000));
            }
            catch
            {
            }
        }
    }
}