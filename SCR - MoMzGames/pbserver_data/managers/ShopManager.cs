/*
 * Arquivo: ShopManager.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 08/08/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.shop;
using Core.server;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.managers
{
    public static class ShopManager
    {
        public static List<GoodItem> ShopAllList = new List<GoodItem>(),
            ShopBuyableList = new List<GoodItem>();
        public static SortedList<int, GoodItem> ShopUniqueList = new SortedList<int, GoodItem>();
        public static List<ShopData> ShopDataMt1 = new List<ShopData>(),
            ShopDataMt2 = new List<ShopData>(),
            ShopDataGoods = new List<ShopData>(),
            ShopDataItems = new List<ShopData>();
        public static int TotalGoods, TotalItems, TotalMatching1, TotalMatching2,
            set4p;
        public static void Load(int type)
        {
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    connection.Open();
                    NpgsqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM shop";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        GoodItem good = new GoodItem
                        {
                            id = data.GetInt32(0),
                            price_gold = data.GetInt32(3),
                            price_cash = data.GetInt32(4),
                            auth_type = data.GetInt32(6),
                            buy_type2 = data.GetInt32(7),
                            buy_type3 = data.GetInt32(8),
                            tag = data.GetInt32(9),
                            title = data.GetInt32(10),
                            visibility = data.GetInt32(11)
                        };

                        good._item.SetItemId(data.GetInt32(1));
                        good._item._name = data.GetString(2);
                        good._item._count = (UInt32)data.GetInt32(5);
                        int Static = ComDiv.getIdStatics(good._item._id, 1);
                        if (type == 1 || type == 2 && Static == 12)
                        {
                            ShopAllList.Add(good);
                            if (good.visibility != 2 && good.visibility != 4)
                                ShopBuyableList.Add(good);
                            if (!ShopUniqueList.ContainsKey(good._item._id) && good.auth_type > 0)
                            {
                                ShopUniqueList.Add(good._item._id, good);
                                if (good.visibility == 4) set4p++;
                            }
                        }
                    }
                    if (type == 1)
                    {
                        LoadDataMatching1Goods(0);
                        LoadDataMatching2(1);
                        LoadDataItems();
                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }
                if (set4p > 0)
                    Logger.warning("[Aviso] Existem " + set4p + " itens na loja invisíveis, porém com os ícones liberados.");
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
        }
        public static void Reset()
        {
            set4p = 0;
            ShopAllList.Clear();
            ShopBuyableList.Clear();
            ShopUniqueList.Clear();
            ShopDataMt1.Clear();
            ShopDataMt2.Clear();
            ShopDataGoods.Clear();
            ShopDataItems.Clear();
            TotalGoods = 0;
            TotalItems = 0;
            TotalMatching1 = 0;
            TotalMatching2 = 0;
        }
        private static void LoadDataMatching1Goods(int cafe)
        {
            List<GoodItem> matchs = new List<GoodItem>(),
                goods = new List<GoodItem>();
            lock (ShopAllList)
            {
                for (int i = 0; i < ShopAllList.Count; i++)
                {
                    GoodItem good = ShopAllList[i];
                    if (good._item._count == 0)
                        continue;
                    if (!(good.tag == 4 && cafe == 0) && (good.tag == 4 && cafe > 0 || good.visibility != 2))
                        matchs.Add(good);
                    if (good.visibility < 2 || good.visibility == 4)
                        goods.Add(good);
                }
            }
            TotalMatching1 = matchs.Count;
            TotalGoods = goods.Count;
            int Pages = (int)Math.Ceiling(matchs.Count / 741d);
            int count = 0;
            for (int i = 0; i < Pages; i++)
            {
                byte[] buffer = getMatchingData(741, i, ref count, matchs);
                ShopData data = new ShopData
                {
                    Buffer = buffer,
                    ItemsCount = count,
                    Offset = (i * 741)
                };
                ShopDataMt1.Add(data);
            }

            Pages = (int)Math.Ceiling(goods.Count / 592d);
            for (int i = 0; i < Pages; i++)
            {
                byte[] buffer = getGoodsData(592, i, ref count, goods);
                ShopData data = new ShopData
                {
                    Buffer = buffer,
                    ItemsCount = count,
                    Offset = (i * 592)
                };
                ShopDataGoods.Add(data);
            }
        }
        private static void LoadDataMatching2(int cafe)
        {
            List<GoodItem> matchs = new List<GoodItem>();
            lock (ShopAllList)
            {
                for (int i = 0; i < ShopAllList.Count; i++)
                {
                    GoodItem good = ShopAllList[i];
                    if (good._item._count == 0)
                        continue;
                    if (!(good.tag == 4 && cafe == 0) && (good.tag == 4 && cafe > 0 || good.visibility != 2))
                        matchs.Add(good);
                }
            }
            TotalMatching2 = matchs.Count;
            int Pages = (int)Math.Ceiling(matchs.Count / 741d);
            int count = 0;
            for (int i = 0; i < Pages; i++)
            {
                byte[] buffer = getMatchingData(741, i, ref count, matchs);
                ShopData data = new ShopData
                {
                    Buffer = buffer,
                    ItemsCount = count,
                    Offset = (i * 741)
                };
                ShopDataMt2.Add(data);
            }
        }
        private static void LoadDataItems()
        {
            List<GoodItem> items = new List<GoodItem>();
            lock (ShopUniqueList)
            {
                for (int i = 0; i < ShopUniqueList.Values.Count; i++)
                {
                    GoodItem good = ShopUniqueList.Values[i];
                    if (good.visibility != 1 && good.visibility != 3)
                        items.Add(good);
                }
            }
            TotalItems = items.Count;
            int ItemsPages = (int)Math.Ceiling(items.Count / 1111d);
            int count = 0;
            for (int i = 0; i < ItemsPages; i++)
            {
                byte[] buffer = getItemsData(1111, i, ref count, items);
                ShopData data = new ShopData
                {
                    Buffer = buffer,
                    ItemsCount = count,
                    Offset = (i * 1111)
                };
                ShopDataItems.Add(data);
            }
        }
        private static byte[] getItemsData(int maximum, int page, ref int count, List<GoodItem> list)
        {
            count = 0;
            using (SendGPacket p = new SendGPacket())
            {
                for (int i = (page * maximum); i < list.Count; i++)
                {
                    WriteItemsData(list[i], p);
                    if (++count == maximum)
                        break;
                }
                return p.mstream.ToArray();
            }
        }
        private static byte[] getGoodsData(int maximum, int page, ref int count, List<GoodItem> list)
        {
            count = 0;
            using (SendGPacket p = new SendGPacket())
            {
                for (int i = (page * maximum); i < list.Count; i++)
                {
                    WriteGoodsData(list[i], p);
                    if (++count == maximum)
                        break;
                }
                return p.mstream.ToArray();
            }
        }
        private static byte[] getMatchingData(int maximum, int page, ref int count, List<GoodItem> list)
        {
            count = 0;
            using (SendGPacket p = new SendGPacket())
            {
                for (int i = (page * maximum); i < list.Count; i++)
                {
                    WriteMatchData(list[i], p);
                    if (++count == maximum)
                        break;
                }
                return p.mstream.ToArray();
            }
        }
        private static void WriteItemsData(GoodItem good, SendGPacket p)
        {
            p.writeD(good._item._id);
            p.writeC((byte)good.auth_type);
            p.writeC((byte)good.buy_type2);
            p.writeC((byte)good.buy_type3);
            p.writeC((byte)good.title);
        }
        private static void WriteGoodsData(GoodItem good, SendGPacket p)
        {
            p.writeD(good.id);
            p.writeC(1);
            p.writeC((byte)(good.visibility == 4 ? 4 : 1));
            //Flag1 = Show icon + Buy option | Flag2 = UNK | Flag4 = Show icon + No buy option
            p.writeD(good.price_gold);
            p.writeD(good.price_cash);
            p.writeC((byte)good.tag);
        }
        private static void WriteMatchData(GoodItem good, SendGPacket p)
        {
            p.writeD(good.id);
            p.writeD(good._item._id);
            p.writeD(good._item._count);
        }
        public static bool IsBlocked(string txt, List<int> items)
        {
            lock (ShopUniqueList)
            {
                for (int i = 0; i < ShopUniqueList.Values.Count; i++)
                {
                    GoodItem good = ShopUniqueList.Values[i];

                    if (!items.Contains(good._item._id) && good._item._name.Contains(txt))//conta dentro do goods os valores escritos em rules
                        items.Add(good._item._id);//adiciona os ids correspodentes a lista itens
                }
            }
            return false;
        }
        public static GoodItem getGood(int goodId)
        {
            if (goodId == 0)
                return null;
            lock (ShopAllList)
            {
                for (int i = 0; i < ShopAllList.Count; i++)
                {
                    GoodItem good = ShopAllList[i];
                    if (good.id == goodId)
                        return good;
                }
            }
            return null;
        }
        public static List<GoodItem> getGoods(List<CartGoods> ShopCart, out int GoldPrice, out int CashPrice)
        {
            GoldPrice = 0;
            CashPrice = 0;
            List<GoodItem> items = new List<GoodItem>();
            if (ShopCart.Count == 0)
                return items;
            lock (ShopBuyableList)
            {
                for (int i = 0; i < ShopBuyableList.Count; i++)
                {
                    GoodItem good = ShopBuyableList[i];
                    for (int i2 = 0; i2 < ShopCart.Count; i2++)
                    {
                        CartGoods CartGood = ShopCart[i2];
                        if (CartGood.GoodId == good.id)
                        {
                            items.Add(good);
                            if (CartGood.BuyType == 1)
                                GoldPrice += good.price_gold;
                            else if (CartGood.BuyType == 2)
                                CashPrice += good.price_cash;
                        }
                    }
                }
            }
            return items;
        }
    }
    public class CartGoods
    {
        public int GoodId, BuyType;
    }
    public class ShopData
    {
        public byte[] Buffer;
        public int ItemsCount, Offset;
    }
}