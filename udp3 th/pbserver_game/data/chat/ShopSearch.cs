using Core;
using Core.managers;
using Core.models.shop;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
    public static class ShopSearch
    {
        public static string SearchGoods(string str, Account player)
        {
            string key = str.Substring(6);
            int count = 0;
            string text = Translation.GetLabel("SearchGoodTitle");
            foreach (GoodItem good in ShopManager.ShopBuyableList)
            {
                if (good._item._name.Contains(key))
                {
                    text += "\n" + Translation.GetLabel("SearchGoodInfo", good.id, good._item._name);
                    if (++count >= 15)
                        break;
                }
            }
            player.SendPacket(new SERVER_MESSAGE_ANNOUNCE_PAK(text));
            return Translation.GetLabel("SearchGoodSuccess", count);
        }
    }
}