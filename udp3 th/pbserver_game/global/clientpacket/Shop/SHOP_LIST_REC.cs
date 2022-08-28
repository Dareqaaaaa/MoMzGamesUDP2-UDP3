using Core;
using Core.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class SHOP_LIST_REC : ReceiveGamePacket
    {
        private int erro;
        public SHOP_LIST_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }
        public override void read()
        {
            erro = readD();
        }
        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null)
                    return;
                if (p.LoadedShop || erro > 0)
                    goto SendPacket;

                p.LoadedShop = true;
                for (int i = 0; i < ShopManager.ShopDataItems.Count; i++)
                {
                    ShopData data = ShopManager.ShopDataItems[i];
                    _client.SendPacket(new SHOP_GET_ITEMS_PAK(data, ShopManager.TotalItems));
                }
                for (int i = 0; i < ShopManager.ShopDataGoods.Count; i++)
                {
                    ShopData data = ShopManager.ShopDataGoods[i];
                    _client.SendPacket(new SHOP_GET_GOODS_PAK(data, ShopManager.TotalGoods));
                }
                _client.SendPacket(new SHOP_GET_REPAIR_PAK());
                _client.SendPacket(new SHOP_TEST2_PAK());
                int cafe = p.pc_cafe;
                if (cafe == 0)
                {
                    for (int i = 0; i < ShopManager.ShopDataMt1.Count; i++)
                    {
                        ShopData data = ShopManager.ShopDataMt1[i];
                        _client.SendPacket(new SHOP_GET_MATCHING_PAK(data, ShopManager.TotalMatching1));
                    }
                }
                else
                {
                    for (int i = 0; i < ShopManager.ShopDataMt2.Count; i++)
                    {
                        ShopData data = ShopManager.ShopDataMt2[i];
                        _client.SendPacket(new SHOP_GET_MATCHING_PAK(data, ShopManager.TotalMatching2));
                    }
                }
            SendPacket:
                _client.SendPacket(new SHOP_LIST_PAK());
            }
            catch (Exception ex)
            {
                Logger.info("SHOP_LIST_REC: " + ex.ToString());
            }
        }
    }
}