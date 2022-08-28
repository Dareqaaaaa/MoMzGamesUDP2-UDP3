using Core;
using Core.managers;
using Core.models.account;
using Core.models.account.clan;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_PLAYER_LEAVE_REC : ReceiveGamePacket
    {
        private uint erro;
        public CLAN_PLAYER_LEAVE_REC(GameClient client, byte[] data)
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
                if (_client == null)
                    return;
                Account p = _client._player;
                try
                {
                    if (p != null && p.clanId > 0)
                    {
                        Clan clan = ClanManager.getClan(p.clanId);
                        if (clan._id > 0 && clan.owner_id != p.player_id)
                        {
                            if (ComDiv.updateDB("accounts", "player_id", p.player_id, new string[] 
                            { 
                                "clan_id", "clanaccess", "clan_game_pt", "clan_wins_pt"
                            }, 0, 0, 0, 0))
                            {
                                using (CLAN_MEMBER_INFO_DELETE_PAK packet = new CLAN_MEMBER_INFO_DELETE_PAK(p.player_id))
                                    ClanManager.SendPacket(packet, p.clanId, p.player_id, true, true);
                                long ownerId = clan.owner_id;
                                if (MessageManager.getMsgsCount(ownerId) < 100)
                                {
                                    Message msg = CreateMessage(clan, p);
                                    if (msg != null)
                                    {
                                        Account pM = AccountManager.getAccount(ownerId, 0);
                                        if (pM != null && pM._isOnline)
                                            pM.SendPacket(new BOX_MESSAGE_RECEIVE_PAK(msg), false);
                                    }
                                }
                                p.clanId = 0;
                                p.clanAccess = 0;
                            }
                            else erro = 0x8000106B;
                        }
                        else erro = 2147487838;
                    }
                    else erro = 2147487835;
                }
                catch
                {
                    erro = 0x8000106B;
                }
                _client.SendPacket(new CLAN_PLAYER_LEAVE_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("[CLAN_PLAYER_LEAVE_REC] " + ex.ToString());
            }
        }
        private Message CreateMessage(Clan clan, Account sender)
        {
            Message msg = new Message(15)
            {
                sender_name = clan._name,
                sender_id = sender.player_id,
                clanId = clan._id,
                type = 4,
                text = sender.player_name,
                state = 1,
                cB = NoteMessageClan.Secession
            };
            return MessageManager.CreateMessage(clan.owner_id, msg) ? msg : null;
        }
    }
}