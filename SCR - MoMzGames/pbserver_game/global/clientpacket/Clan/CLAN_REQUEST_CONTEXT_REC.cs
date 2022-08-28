using Core;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_REQUEST_CONTEXT_REC : ReceiveGamePacket
    {
        public CLAN_REQUEST_CONTEXT_REC(GameClient client, byte[] data)
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
                Account player = _client._player;
                if (player == null)
                    return;
                _client.SendPacket(new CLAN_REQUEST_CONTEXT_PAK(player.clanId));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}