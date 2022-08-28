using Core;
using Core.managers;
using Core.models.account;
using Core.models.shop;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BOX_MESSAGE_GIFT_TAKE_REC : ReceiveGamePacket
    {
        private int msgId;
        public BOX_MESSAGE_GIFT_TAKE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            msgId = readD();
        }

        public override void run()
        {
            try
            {
                if (_client == null)
                    return;
                Account p = _client._player;
                if (p == null)
                    return;
                if (p._inventory._items.Count >= 500)
                {
                    _client.SendPacket(new INVENTORY_ITEM_EQUIP_PAK(2147487785));
                    _client.SendPacket(new BOX_MESSAGE_GIFT_TAKE_PAK(0x80000000));
                }
                else
                {
                    Message msg = MessageManager.getMessage(msgId, p.player_id);
                    if (msg != null && msg.type == 2)
                    {
                        GoodItem good = ShopManager.getGood((int)msg.sender_id);
                        if (good != null)
                        {
                            Logger.warning("Received gift. [Good: " + good.id + "; Item: " + good._item._id + "]");
                            _client.SendPacket(new BOX_MESSAGE_GIFT_TAKE_PAK(1, good._item, p));
                            MessageManager.DeleteMessage(msgId, p.player_id);
                        }
                    }
                    else
                        _client.SendPacket(new BOX_MESSAGE_GIFT_TAKE_PAK(0x80000000));
                }
            }
            catch (Exception ex)
            {
                Logger.info("[BOX_MESSAGE_GIFT_TAKE_REC] " + ex.ToString());
            }
        }
    }
}