using Core;
using Core.managers;
using Core.models.shop;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class SHOP_BUY_ITEM_REC : ReceiveGamePacket
    {
        private List<CartGoods> ShopCart = new List<CartGoods>();
        public SHOP_BUY_ITEM_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }
        public override void read()
        {
            int count = readC();
            for (int i = 0; i < count; i++)
            {
                ShopCart.Add(new CartGoods
                {
                    GoodId = readD(),
                    BuyType = readC()
                });
            }
        }
        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null || p.player_name.Length == 0)
                {
                    _client.SendPacket(new SHOP_BUY_PAK(2147487767));
                    return;
                }
                if (p._inventory._items.Count >= 500)
                {
                    _client.SendPacket(new SHOP_BUY_PAK(2147487929));
                    return;
                }
                int gold, cash;
                List<GoodItem> items = ShopManager.getGoods(ShopCart, out gold, out cash);
                if (items.Count == 0)
                    _client.SendPacket(new SHOP_BUY_PAK(2147487767));
                else if (0 > (p._gp - gold) || 0 > (p._money - cash))
                    _client.SendPacket(new SHOP_BUY_PAK(2147487768));
                else if (PlayerManager.updateAccountCashing(p.player_id, (p._gp - gold), (p._money - cash)))
                {
                    p._gp -= gold;
                    p._money -= cash;
                    _client.SendPacket(new SHOP_BUY_PAK(1, items, p));
                }
                else
                    _client.SendPacket(new SHOP_BUY_PAK(2147487769));
            }
            catch (Exception ex)
            {
                Logger.info("SHOP_BUY_ITEM_REC: " + ex.ToString());
            }
        }
    }
}