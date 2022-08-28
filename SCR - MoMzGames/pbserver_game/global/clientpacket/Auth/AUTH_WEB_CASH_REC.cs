using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class AUTH_WEB_CASH_REC : ReceiveGamePacket
    {
        public AUTH_WEB_CASH_REC(GameClient client, byte[] data)
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
                Account p = _client._player;
                if (p == null)
                    return;
                _client.SendPacket(new AUTH_WEB_CASH_PAK(0, p._gp, p._money));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}