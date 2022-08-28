using Core;
using Core.managers;
using Core.models.account;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BOX_MESSAGE_CREATE_REC : ReceiveGamePacket
    {
        private int nameLength, textLength;
        private string name, text;
        private uint erro;
        public BOX_MESSAGE_CREATE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            nameLength = readC();
            textLength = readC();
            name = readS(nameLength);
            text = readS(textLength);
        }

        public override void run()
        {
            try
            {
                if (text.Length > 120)
                    return;
                //0x80001080 STR_TBL_NETWORK_DONT_SEND_MYSELF_MESSAGE
                //0x80001081 STR_TBL_NETWORK_FULL_SEND_MESSAGE_PER_DAY
                Account p = _client._player;
                if (p == null || p.player_name.Length == 0 || p.player_name == name)
                    return;
                Account rec = AccountManager.getAccount(name, 1, 0);
                if (rec != null)
                {
                    if (MessageManager.getMsgsCount(rec.player_id) >= 100)
                        erro = 0x8000107F;
                    else
                    {
                        Message msg = CreateMessage(p.player_name, rec.player_id, _client.player_id);
                        if (msg != null)
                            rec.SendPacket(new BOX_MESSAGE_RECEIVE_PAK(msg), false);
                    }
                }
                else erro = 0x8000107E;
                _client.SendPacket(new BOX_MESSAGE_CREATE_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("[BOX_MESSAGE_CREATE_REC] " + ex.ToString());
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