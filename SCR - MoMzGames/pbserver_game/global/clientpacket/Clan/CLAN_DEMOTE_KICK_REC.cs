using Core;
using Core.managers;
using Core.models.account;
using Core.models.account.clan;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class CLAN_DEMOTE_KICK_REC : ReceiveGamePacket
    {
        private uint result;
        public CLAN_DEMOTE_KICK_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            Account player = _client._player;
            if (player == null)
                return;
            Clan clan = ClanManager.getClan(player.clanId);
            if (clan._id == 0 || !(player.clanAccess >= 1 && player.clanAccess <= 2 || clan.owner_id == _client.player_id))
            {
                result = 2147487833;
                return;
            }
            List<Account> clanPlayers = ClanManager.getClanPlayers(clan._id, -1, true);
            int countPlayers = readC();
            for (int i = 0; i < countPlayers; i++)
            {
                Account member = AccountManager.getAccount(readQ(), 0);
                if (member != null && member.clanId == clan._id && member._match == null && ComDiv.updateDB("accounts", "player_id", member.player_id, new string[] 
                { 
                    "clan_id", "clanaccess", "clan_game_pt", "clan_wins_pt"
                }, 0, 0, 0, 0))
                {
                    using (CLAN_MEMBER_INFO_DELETE_PAK packet = new CLAN_MEMBER_INFO_DELETE_PAK(member.player_id))
                        ClanManager.SendPacket(packet, clanPlayers, member.player_id);
                    member.clanId = 0;
                    member.clanAccess = 0;
                    SEND_CLAN_INFOS.Load(member, null, 0);
                    if (MessageManager.getMsgsCount(member.player_id) < 100)
                    {
                        Message msg = CreateMessage(clan, member.player_id, _client.player_id);
                        if (msg != null && member._isOnline)
                            member.SendPacket(new BOX_MESSAGE_RECEIVE_PAK(msg), false);
                    }
                    if (member._isOnline)
                        member.SendPacket(new CLAN_PRIVILEGES_KICK_PAK(), false);
                    result++;
                    clanPlayers.Remove(member);
                }
                else
                { result = 2147487833; break; }
            }
        }

        public override void run()
        {
            try
            {
                if (_client != null)
                    _client.SendPacket(new CLAN_DEPORTATION_PAK(result));
            }
            catch (Exception ex)
            {
                Logger.info("[CLAN_DEMOTE_KICK_REC] " + ex.ToString());
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
                state = 1,
                cB = NoteMessageClan.Deportation
            };
            return MessageManager.CreateMessage(owner, msg) ? msg : null;
        }
    }
}