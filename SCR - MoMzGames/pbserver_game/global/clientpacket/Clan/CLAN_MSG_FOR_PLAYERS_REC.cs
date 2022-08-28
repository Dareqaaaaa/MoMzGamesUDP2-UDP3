using Core;
using Core.managers;
using Core.models.account;
using Core.models.account.clan;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class CLAN_MSG_FOR_PLAYERS_REC : ReceiveGamePacket
    {
        private int type;
        private string message;
        public CLAN_MSG_FOR_PLAYERS_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            type = readC();
            message = readS(readC());
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (message.Length > 120 || player == null)
                    return;
                Clan clan = ClanManager.getClan(player.clanId);
                int playersLoaded = 0;
                if (clan._id > 0 && clan.owner_id == _client.player_id)
                {
                    List<Account> players = ClanManager.getClanPlayers(clan._id, _client.player_id, true);
                    for (int i = 0; i < players.Count; i++)
                    {
                        Account member = players[i];
                        if ((type == 0 || member.clanAccess == 2 && type == 1 || member.clanAccess == 3 && type == 2) && MessageManager.getMsgsCount(member.player_id) < 100)
                        {
                            ++playersLoaded;
                            Message msg = CreateMessage(clan, member.player_id, _client.player_id);
                            if (msg != null && member._isOnline)
                                member.SendPacket(new BOX_MESSAGE_RECEIVE_PAK(msg), false);
                        }
                    }
                }
                _client.SendPacket(new CLAN_MSG_FOR_PLAYERS_PAK(playersLoaded));
                if (playersLoaded > 0)
                    _client.SendPacket(new BOX_MESSAGE_SEND_PAK(0));
            }
            catch (Exception ex)
            {
                Logger.info("CLAN_MSG_FOR_PLAYERS_REC: " + ex.ToString());
            }
        }
        private Message CreateMessage(Clan clan, long owner, long senderId)
        {
            Message msg = new Message(15)
            {
                sender_name = clan._name,
                sender_id = senderId,
                clanId = clan._id,
                type = 4,
                text = message,
                state = 1
            };
            return MessageManager.CreateMessage(owner, msg) ? msg : null;
        }
    }
}