using Core;
using Core.managers;
using Core.models.account;
using Core.models.enums;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_MESSAGE_INVITE_REC : ReceiveGamePacket
    {
        private uint erro;
        private int type;
        private long objId;
        public CLAN_MESSAGE_INVITE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            type = readC(); //0 - AMIGOS (Q) || 1 - SALA (D) || 2 - LOBBY (D)
            if (type == 0)
                objId = readQ();
            else if (type == 1)
                objId = readD();
            else if (type == 2)
                objId = readUD();
            else readD();
        }

        public override void run()
        {
            Account p = _client._player;
            if (p == null || p.clanId == 0)
                return;
            try
            {
                if (type == 0)
                {
                    Account f = AccountManager.getAccount(objId, true);
                    if (f != null)
                        SendBoxMessage(f, p.clanId);
                    else erro = 0x80000000;
                }
                else if (type == 1)
                {
                    Room room = p._room;
                    if (room != null)
                    {
                        Account player = room.getPlayerBySlot((int)objId);
                        if (player != null)
                            SendBoxMessage(player, p.clanId);
                        else erro = 0x80000000;
                    }
                    else erro = 0x80000000;
                }
                else if (type == 2)
                {
                    Channel ch = p.getChannel();
                    if (ch != null)
                    {
                        PlayerSession ps = ch.getPlayer((uint)objId);
                        long pId = ps != null ? ps._playerId : -1;
                        if (pId != -1 && pId != _client.player_id)
                        {
                            Account player = AccountManager.getAccount(pId, true);
                            if (player != null)
                                SendBoxMessage(player, p.clanId);
                            else erro = 0x80000000;
                        }
                        else erro = 0x80000000;
                    }
                    else erro = 0x80000000;
                }
                _client.SendPacket(new CLAN_MESSAGE_INVITE_PAK(erro));
            }
            catch (Exception e)
            {
                Logger.error(e.ToString());
            }
        }
        private void SendBoxMessage(Account player, int clanId)
        {
            if (MessageManager.getMsgsCount(player.player_id) >= 100)
                erro = 0x80000000;
            else
            {
                Message msg = CreateMessage(clanId, player.player_id, _client.player_id);
                if (msg != null && player._isOnline)
                    player.SendPacket(new BOX_MESSAGE_RECEIVE_PAK(msg), false);
            }
        }
        private Message CreateMessage(int clanId, long owner, long senderId)
        {
            Message msg = new Message(15)
            {
                sender_name = ClanManager.getClan(clanId)._name,
                clanId = clanId,
                sender_id = senderId,
                type = 5,
                state = 1,
                cB = NoteMessageClan.Invite
            };
            return MessageManager.CreateMessage(owner, msg) ? msg : null;
        }
    }
}