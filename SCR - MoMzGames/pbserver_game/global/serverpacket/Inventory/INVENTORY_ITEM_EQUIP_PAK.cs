using Core.managers;
using Core.models.account.players;
using Core.server;
using Game.data.model;
using System;

namespace Game.global.serverpacket
{
    public class INVENTORY_ITEM_EQUIP_PAK : SendPacket
    {
        private uint erro;
        private ItemsModel item;
        public INVENTORY_ITEM_EQUIP_PAK(uint erro, ItemsModel item = null, Account p = null)
        {
            this.erro = erro;
            if (erro != 1)
                return;
            if (item != null)
            {
                int wclass = ComDiv.getIdStatics(item._id, 1);
                if (wclass == 13 || wclass == 15)
                {
                    if (item._count > 1 && item._equip == 1)
                        ComDiv.updateDB("player_items", "count", (long)--item._count, "object_id", item._objId, "owner_id", p.player_id);
                    else
                    {
                        PlayerManager.DeleteItem(item._objId, p.player_id);
                        p._inventory.RemoveItem(item);
                        item._id = 0;
                        item._count = 0;
                    }
                }
                else item._equip = 2;
                this.item = item;
            }
            else this.erro = 0x80000000;
        }
        public override void write()
        {
            writeH(535);
            writeD(erro);
            if (erro == 1)
            {
                writeD(uint.Parse(DateTime.Now.ToString("yyMMddHHmm")));
                writeQ(item._objId);
                writeD(item._id);
                writeC((byte)item._equip);
                writeD(item._count);
            }
            //0x80001086 STR_TBL_GUI_BASE_NO_EQUIP_PRE_DESIGNATION
        }
    }
}