using Core;
using Core.managers;
using Core.models.account.players;
using Core.models.shop;
using Core.server;
using Game.data.model;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Game.global.serverpacket
{
    public class SHOP_BUY_PAK : SendPacket
    {
        private List<ItemsModel> weapons = new List<ItemsModel>(),
            charas = new List<ItemsModel>(),
            cupons = new List<ItemsModel>();
        private Account p;
        private uint erro;
        public SHOP_BUY_PAK(uint erro, List<GoodItem> item = null, Account player = null)
        {
            this.erro = erro;
            if (this.erro == 1)
            {
                p = player;
                AddItems(item);
            }
        }
        public override void write()
        {
            writeH(531);
            writeD(erro);
            if (erro == 1)
            {
                writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
                writeD(charas.Count);
                writeD(weapons.Count);
                writeD(cupons.Count);
                foreach (ItemsModel item in charas)
                {
                    writeQ(item._objId);
                    writeD(item._id);
                    writeC((byte)item._equip);
                    writeD(item._count);
                }
                foreach (ItemsModel item in weapons)
                {
                    writeQ(item._objId);
                    writeD(item._id);
                    writeC((byte)item._equip);
                    writeD(item._count);
                }
                foreach (ItemsModel item in cupons)
                {
                    writeQ(item._objId);
                    writeD(item._id);
                    writeC((byte)item._equip);
                    writeD(item._count);
                }
                writeD(p._gp);
                writeD(p._money);
            }
        }
        private void AddItems(List<GoodItem> items)
        {
            GoodItem g2 = null;
            try
            {
                foreach (GoodItem good in items)
                {
                    g2 = good;
                    ItemsModel iv = p._inventory.getItem(good._item._id);

                    ItemsModel modelo = new ItemsModel(good._item);
                    if (iv == null)
                    {
                        if (PlayerManager.CreateItem(modelo, p.player_id))
                            p._inventory.AddItem(modelo);
                    }
                    else
                    {
                        modelo._count = iv._count;
                        modelo._objId = iv._objId;
                        if (iv._equip == 1)
                        {
                            modelo._count += good._item._count;
                            ComDiv.updateDB("player_items", "count", (long)modelo._count, "owner_id", p.player_id, "item_id", modelo._id);
                        }
                        else if (iv._equip == 2 && modelo._category != 3)
                        {
                            DateTime data = DateTime.ParseExact(iv._count.ToString(), "yyMMddHHmm", CultureInfo.InvariantCulture);
                            modelo._count = uint.Parse(data.AddSeconds(good._item._count).ToString("yyMMddHHmm"));
                            ComDiv.updateDB("player_items", "count", (long)modelo._count, "owner_id", p.player_id, "item_id", modelo._id);
                        }
                        modelo._equip = iv._equip;
                        iv._count = modelo._count;
                    }
                    if (modelo._category == 1)
                        weapons.Add(modelo);
                    else if (modelo._category == 2)
                        charas.Add(modelo);
                    else if (modelo._category == 3)
                        cupons.Add(modelo);
                }
            }
            catch (Exception ex)
            {
                erro = 2147487767;
                Logger.warning("[SHOP_BUY_PAK] " + ex.ToString());
                if (g2 != null)
                    Logger.warning("[SHOP_BUY_PAK] Good: " + g2.id);
            }
        }
    }
}