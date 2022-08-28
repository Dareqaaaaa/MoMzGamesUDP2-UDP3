using Core;
using Core.managers;
using Core.models.account;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BOX_MESSAGE_REPLY_REC : ReceiveGamePacket
    {
        private long receiverId;
        private string text;
        private uint erro;
        public BOX_MESSAGE_REPLY_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            receiverId = readQ();
            text = readS(readC());
        }

        public override void run()
        {
            try
            {
                if (text.Length > 120)
                    return;
                Account p = _client._player;
                if (p == null || _client.player_id == receiverId)
                    return;
                Account rec = AccountManager.getAccount(receiverId, 0);
                if (rec != null)
                {
                    if (MessageManager.getMsgsCount(rec.player_id) >= 100)
                        erro = 2147487871;
                    else
                    {
                        Message msgF = CreateMessage(p.player_name, rec.player_id, _client.player_id);
                        if (msgF != null)
                            rec.SendPacket(new BOX_MESSAGE_RECEIVE_PAK(msgF), false);
                    }
                }
                else erro = 2147487870;
                _client.SendPacket(new BOX_MESSAGE_CREATE_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("[BOX_MESSAGE_RESPOND_REC] " + ex.ToString());
            }
        }
        private Message CreateMessage(string senderName, long owner, long senderId)
        {
            Message msg = new Message(15)
            {
                sender_name = senderName,
                sender_id = senderId,
                text = text,
                state = 1
            };
            if (!MessageManager.CreateMessage(owner, msg))
            {
                erro = 0x80000000;
                return null;
            }
            return msg;
        }
    }
}