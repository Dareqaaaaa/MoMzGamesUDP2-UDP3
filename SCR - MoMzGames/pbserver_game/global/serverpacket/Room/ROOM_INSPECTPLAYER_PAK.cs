using Core.models.account.players;
using Core.server;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class ROOM_INSPECTPLAYER_PAK : SendPacket
    {
        private Account p;
        public ROOM_INSPECTPLAYER_PAK(Account p)
        {
            this.p = p;
        }

        public override void write()
        {
            writeH(3893);
            writeD(p._equip._primary);
            writeD(p._equip._secondary);
            writeD(p._equip._melee);
            writeD(p._equip._grenade);
            writeD(p._equip._special);
            writeD(p._equip._red);
            writeD(p._equip._blue);
            writeD(p._equip._helmet);
            writeD(p._equip._beret);
            writeD(p._equip._dino);
            List<ItemsModel> cupons = p._inventory.getItemsByType(4);
            writeD(cupons.Count);
            foreach (ItemsModel item in cupons)
                writeD(item._id);
        }
    }
}