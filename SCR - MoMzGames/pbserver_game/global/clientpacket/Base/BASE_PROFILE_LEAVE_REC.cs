using Core;
using Core.models.enums;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BASE_PROFILE_LEAVE_REC : ReceiveGamePacket
    {
        public BASE_PROFILE_LEAVE_REC(GameClient client, byte[] data)
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
                if (p == null)
                    return;
                Room room = p._room;
                if (room != null)
                    room.changeSlotState(p._slotId, SLOT_STATE.NORMAL, true);
                _client.SendPacket(new BASE_PROFILE_LEAVE_PAK());
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}