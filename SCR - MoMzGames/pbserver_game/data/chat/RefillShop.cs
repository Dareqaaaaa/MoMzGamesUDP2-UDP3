using Core;
using Core.managers;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
    public static class RefillShop
    {
        public static string SimpleRefill(Account player)
        {
            ShopManager.Reset();
            ShopManager.Load(1);
            Logger.warning(Translation.GetLabel("RefillShopWarn", player.player_name));
            return Translation.GetLabel("RefillShopMsg");
        }

        public static string InstantRefill(Account player)
        {
            ShopManager.Reset();
            ShopManager.Load(1);

            for (int i = 0; i < ShopManager.ShopDataItems.Count; i++)
            {
                ShopData data = ShopManager.ShopDataItems[i];
                player.SendPacket(new SHOP_GET_ITEMS_PAK(data, ShopManager.TotalItems));
            }
            for (int i = 0; i < ShopManager.ShopDataGoods.Count; i++)
            {
                ShopData data = ShopManager.ShopDataGoods[i];
                player.SendPacket(new SHOP_GET_GOODS_PAK(data, ShopManager.TotalGoods));
            }
            player.SendPacket(new SHOP_GET_REPAIR_PAK());
            player.SendPacket(new SHOP_TEST2_PAK());
            int cafe = player.pc_cafe;
            if (cafe == 0)
            {
                for (int i = 0; i < ShopManager.ShopDataMt1.Count; i++)
                {
                    ShopData data = ShopManager.ShopDataMt1[i];
                    player.SendPacket(new SHOP_GET_MATCHING_PAK(data, ShopManager.TotalMatching1));
                }
            }
            else
            {
                for (int i = 0; i < ShopManager.ShopDataMt2.Count; i++)
                {
                    ShopData data = ShopManager.ShopDataMt2[i];
                    player.SendPacket(new SHOP_GET_MATCHING_PAK(data, ShopManager.TotalMatching2));
                }
            }
            player.SendPacket(new SHOP_LIST_PAK());

            Logger.warning(Translation.GetLabel("RefillShopWarn", player.player_name));
            return Translation.GetLabel("RefillShopMsg");
        }
    }
}