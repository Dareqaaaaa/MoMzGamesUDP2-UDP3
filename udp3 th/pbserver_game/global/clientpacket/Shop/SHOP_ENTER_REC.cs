using Core;
using Core.models.enums;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class SHOP_ENTER_REC : ReceiveGamePacket
    {
        private int unk;
        public SHOP_ENTER_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            unk = readD(); //?,
        }

        public override void run()
        {
            try
            {
                if (_client == null)
                    return;
                Account p = _client._player;
                Room room = p == null ? null : p._room;
                if (room != null)
                {
                    room.changeSlotState(p._slotId, SLOT_STATE.SHOP, false);
                    room.StopCountDown(p._slotId);
                    room.updateSlotsInfo();
                }
                _client.SendPacket(new SHOP_ENTER_PAK());
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}