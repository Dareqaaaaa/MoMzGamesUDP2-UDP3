using Core;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BASE_GET_USER_STATS_REC : ReceiveGamePacket
    {
        private long objId;
        public BASE_GET_USER_STATS_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            objId = readQ();
        }

        public override void run()
        {
            if (_client._player == null)
                return;
            try
            {
                Account player = AccountManager.getAccount(objId, 0);
                _client.SendPacket(new BASE_GET_USER_STATS_PAK(player != null ? player._statistic : null));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}