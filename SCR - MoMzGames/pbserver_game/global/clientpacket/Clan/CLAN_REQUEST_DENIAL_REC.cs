using Core;
using Core.managers;
using Core.models.account;
using Core.models.account.clan;
using Core.models.enums;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_REQUEST_DENIAL_REC : ReceiveGamePacket
    {
        private int result;
        public CLAN_REQUEST_DENIAL_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            Account p = _client._player;
            if (p == null)
                return;
            Clan clan = ClanManager.getClan(p.clanId);
            if (clan._id > 0 && (p.clanAccess >= 1 && p.clanAccess <= 2 || clan.owner_id == p.player_id))
            {
                int count = readC();
                for (int i = 0; i < count; i++)
                {
                    long pId = readQ();
                    if (PlayerManager.DeleteInviteDb(clan._id, pId))
                    {
                        if (MessageManager.getMsgsCount(pId) < 100)
                        {
                            Message msg = CreateMessage(clan, pId, p.player_id);
                            if (msg != null)
                            {
                                Account pK = AccountManager.getAccount(pId, 0);
                                if (pK != null && pK._isOnline)
                                    pK.SendPacket(new BOX_MESSAGE_RECEIVE_PAK(msg), false);
                            }
                        }
                        result++;
                    }
                }
            }
        }

        public override void run()
        {
            try
            {
                _client.SendPacket(new CLAN_REQUEST_DENIAL_PAK(result));
            }
            catch (Exception ex)
            {
                Logger.info("[CLAN_REQUEST_DENIAL_REC] " + ex.ToString());
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
                cB = NoteMessageClan.InviteDenial
            };
            return MessageManager.CreateMessage(owner, msg) ? msg : null;
        }
    }
}