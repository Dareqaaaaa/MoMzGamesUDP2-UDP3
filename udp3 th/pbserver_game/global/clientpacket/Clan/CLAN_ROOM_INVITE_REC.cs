using Core;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_ROOM_INVITED_REC : ReceiveGamePacket
    {
        private long pId;
        public CLAN_ROOM_INVITED_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            pId = readQ();
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player == null || player.clanId == 0)
                    return;
                Account member = AccountManager.getAccount(pId, 0); //Não usar DB?
                if (member != null && member.clanId == player.clanId)
                    member.SendPacket(new CLAN_ROOM_INVITE_RESULT_PAK(_client.player_id), false);
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}