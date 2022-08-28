/*
 * Arquivo: BASE_INVENTORY_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 19/11/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account.players;
using Core.server;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
    public class BASE_USER_INVENTORY_PAK : SendPacket
    {
        private List<ItemsModel> charas = new List<ItemsModel>(),
            weapons = new List<ItemsModel>(),
            cupons = new List<ItemsModel>();
        public BASE_USER_INVENTORY_PAK(List<ItemsModel> items)
        {
            InventoryLoad(items);
        }
        private void InventoryLoad(List<ItemsModel> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                ItemsModel item = items[i];
                if (item._category == 1)
                    weapons.Add(item);
                else if (item._category == 2)
                    charas.Add(item);
                else if (item._category == 3)
                    cupons.Add(item);
            }
        }
        public override void write()
        {
            writeH(2699);
            writeD(charas.Count);
            foreach (ItemsModel item in charas)
            {
                writeQ(item._objId);
                writeD(item._id);
                writeC((byte)item._equip);
                writeD(item._count);
            }
            writeD(weapons.Count);
            foreach (ItemsModel item in weapons)
            {
                writeQ(item._objId);
                writeD(item._id);
                writeC((byte)item._equip);
                writeD(item._count);
            }
            writeD(cupons.Count);
            foreach (ItemsModel item in cupons)
            {
                writeQ(item._objId);
                writeD(item._id);
                writeC((byte)item._equip);
                writeD(item._count);
            }
            writeD(0);
        }
    }
}