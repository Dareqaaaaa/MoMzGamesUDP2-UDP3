using Core;
using Core.models.enums;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class CLAN_CLIENT_LEAVE_REC : ReceiveGamePacket
    {
        public CLAN_CLIENT_LEAVE_REC(GameClient client, byte[] data)
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
                _client.SendPacket(new CLAN_CLIENT_LEAVE_PAK());
            }
            catch (Exception ex)
            {
                Logger.info("CLAN_CLIENT_LEAVE_REC: " + ex.ToString());
            }
        }
    }
}