using Core.managers;
using Core.models.account.players;
using Core.server;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class BOX_MESSAGE_GIFT_TAKE_PAK : SendPacket
    {
        private uint _erro;
        private List<ItemsModel> charas = new List<ItemsModel>(),
            weapons = new List<ItemsModel>(),
            cupons = new List<ItemsModel>();
        public BOX_MESSAGE_GIFT_TAKE_PAK(uint erro, ItemsModel item = null, Account p = null)
        {
            _erro = erro;
            if (_erro == 1)
                get(item, p);
        }

        public override void write()
        {
            writeH(541);
            writeD(_erro); //2231369729 - erro | 1 - sucesso
            if (_erro == 1)
            {
                writeD(charas.Count);
                writeD(weapons.Count);
                writeD(cupons.Count);
                writeD(0);
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
        }

        private void get(ItemsModel item, Account p)
        {
            try
            {
                ItemsModel modelo = new ItemsModel(item) { _objId = item._objId };
                PlayerManager.tryCreateItem(modelo, p._inventory, p.player_id);
                if (modelo._category == 1)
                    weapons.Add(modelo);
                else if (modelo._category == 2)
                    charas.Add(modelo);
                else if (modelo._category == 3)
                    cupons.Add(modelo);
            }
            catch
            { p.Close(0); }
        }
    }
}