using Core;
using Core.managers;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class AUTH_CHECK_NICKNAME_REC : ReceiveGamePacket
    {
        private string name;
        public AUTH_CHECK_NICKNAME_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            name = readS(33);
        }

        public override void run()
        {
            try
            {
                if (_client == null || _client._player == null)
                    return;
                _client.SendPacket(new AUTH_CHECK_NICKNAME_PAK(!PlayerManager.isPlayerNameExist(name) ? 0 : 2147483923));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}