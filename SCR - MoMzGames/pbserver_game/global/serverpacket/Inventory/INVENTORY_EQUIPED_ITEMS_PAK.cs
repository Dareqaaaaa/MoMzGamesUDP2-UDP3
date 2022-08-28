using Core.managers;
using Core.models.account.players;
using Core.models.enums.flags;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class INVENTORY_EQUIPED_ITEMS_PAK : SendPacket
    {
        private InventoryFlag type;
        private PlayerEquipedItems equip;
        /// <summary>
        /// Gera um pacote que faz uma checagem em todos os itens equipados, comparando-os com o inventário.
        /// </summary>
        /// <param name="p">Conta</param>
        public INVENTORY_EQUIPED_ITEMS_PAK(Account p)
        {
            this.type = (InventoryFlag)PlayerManager.CheckEquipedItems(p._equip, p._inventory._items);
            equip = p._equip;
        }
        public INVENTORY_EQUIPED_ITEMS_PAK(Account p, int type)
        {
            this.type = (InventoryFlag)type;
            equip = p._equip;
        }

        public override void write()
        {
            writeH(2058);
            writeD((int)type);
            if (type.HasFlag(InventoryFlag.Character))
            {
                writeD(equip._red);
                writeD(equip._blue);
                writeD(equip._helmet);
                writeD(equip._beret);
                writeD(equip._dino);
            }
            if (type.HasFlag(InventoryFlag.Weapon))
            {
                writeD(equip._primary);
                writeD(equip._secondary);
                writeD(equip._melee);
                writeD(equip._grenade);
                writeD(equip._special);
            }
        }
    }
}