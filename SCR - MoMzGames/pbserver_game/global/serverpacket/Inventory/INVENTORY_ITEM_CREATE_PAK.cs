using Core;
using Core.managers;
using Core.models.account.players;
using Core.server;
using Game.data.model;
using Game.data.sync.server_side;
using System;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class INVENTORY_ITEM_CREATE_PAK : SendPacket
    {
        private int _type;
        private List<ItemsModel> weapons = new List<ItemsModel>(), 
            charas = new List<ItemsModel>(), 
            cupons = new List<ItemsModel>();
        public INVENTORY_ITEM_CREATE_PAK(int type, Account player, List<ItemsModel> items)
        {
            _type = type;
            AddItems(player, items);
        }
        public INVENTORY_ITEM_CREATE_PAK(int type, Account player, ItemsModel item)
        {
            _type = type;
            AddItems(player, item);
        }
        public override void write()
        {
            writeH(3588);
            writeC((byte)_type);
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
        }

        private void AddItems(Account p, List<ItemsModel> items)
        {
            try
            {
                for (int i = 0; i < items.Count; i++)
                {
                    ItemsModel item = items[i];
                    ItemsModel modelo = new ItemsModel(item) { _objId = item._objId };
                    if (_type == 1)
                        PlayerManager.tryCreateItem(modelo, p._inventory, p.player_id);
                    SEND_ITEM_INFO.LoadItem(p, modelo);
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
                p.Close(0);
                Logger.warning("[INVENTORY_ITEM_CREATE_PAK1] " + ex.ToString());
            }
        }
        private void AddItems(Account p, ItemsModel item)
        {
            try
            {
                ItemsModel modelo = new ItemsModel(item) { _objId = item._objId };
                if (_type == 1)
                    PlayerManager.tryCreateItem(modelo, p._inventory, p.player_id);
                SEND_ITEM_INFO.LoadItem(p, modelo);
                if (modelo._category == 1)
                    weapons.Add(modelo);
                else if (modelo._category == 2)
                    charas.Add(modelo);
                else if (modelo._category == 3)
                    cupons.Add(modelo);
            }
            catch (Exception ex)
            {
                p.Close(0);
                Logger.warning("[INVENTORY_ITEM_CREATE_PAK2] " + ex.ToString());
            }
        }
    }
}