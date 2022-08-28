/*
 * Arquivo: GoodItem.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 15/10/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account.players;

namespace Core.models.shop
{
    public class GoodItem
    {
        public int price_gold, price_cash, auth_type, buy_type2, buy_type3, id, tag, title, visibility;
        public ItemsModel _item = new ItemsModel { _equip = 1 };
    }
}