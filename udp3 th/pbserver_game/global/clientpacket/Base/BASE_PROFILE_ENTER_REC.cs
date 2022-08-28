using Core;
using Core.models.enums;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BASE_PROFILE_ENTER_REC : ReceiveGamePacket
    {
        public BASE_PROFILE_ENTER_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                Room room = p == null ? null : p._room;
                if (room != null)
                {
                    room.changeSlotState(p._slotId, SLOT_STATE.INFO, false);
                    room.StopCountDown(p._slotId);
                    room.updateSlotsInfo();
                }
                _client.SendPacket(new BASE_PROFILE_ENTER_PAK());
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }
    }
}