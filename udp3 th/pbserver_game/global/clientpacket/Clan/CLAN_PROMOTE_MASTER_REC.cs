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
    public class CLAN_PROMOTE_MASTER_REC : ReceiveGamePacket
    {
        private long memberId;
        private uint erro;
        public CLAN_PROMOTE_MASTER_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            memberId = readQ();
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null || p.clanAccess != 1)
                    return;
                Account member = AccountManager.getAccount(memberId, 0);
                int clanId = p.clanId;
                if (member == null || member.clanId != clanId)
                    erro = 0x80000000;
                else if (member._rank > 10)
                {
                    Clan clan = ClanManager.getClan(clanId);
                    if (clan._id > 0 && clan.owner_id == _client.player_id && member.clanAccess == 2 && ComDiv.updateDB("clan_data", "owner_id", memberId, "clan_id", clanId) &&
                        ComDiv.updateDB("accounts", "clanaccess", 1, "player_id", memberId) &&
                        ComDiv.updateDB("accounts", "clanaccess", 2, "player_id", p.player_id))
                    {
                        member.clanAccess = 1; //Dono
                        p.clanAccess = 2; //Aux
                        clan.owner_id = memberId;
                        if (MessageManager.getMsgsCount(member.player_id) < 100)
                        {
                            Message msg = CreateMessage(clan, member.player_id, p.player_id);
                            if (msg != null && member._isOnline)
                                member.SendPacket(new BOX_MESSAGE_RECEIVE_PAK(msg), false);
                        }
                        if (member._isOnline)
                            member.SendPacket(new CLAN_PRIVILEGES_MASTER_PAK(), false);
                    }
                    else erro = 2147487744;
                }
                else erro = 2147487928;
                _client.SendPacket(new CLAN_COMMISSION_MASTER_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("[CLAN_PROMOTE_MASTER_REC] " + ex.ToString());
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
                cB = NoteMessageClan.Master
            };
            return MessageManager.CreateMessage(owner, msg) ? msg : null;
        }
    }
}