using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.data.utils;
using System;

namespace Game.global.clientpacket
{
    public class BATTLE_LOADING_REC : ReceiveGamePacket
    {
        private string name;
        public BATTLE_LOADING_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            name = readS(readC());
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null)
                    return;
                Room room = p._room;
                SLOT slot;
                if (room != null && room.isPreparing() && room.getSlot(p._slotId, out slot) && slot.state == SLOT_STATE.LOAD)
                {
                    slot.preLoadDate = DateTime.Now;
                    room.StartCounter(0, p, slot);
                    room.changeSlotState(slot, SLOT_STATE.RENDEZVOUS, true);
                    room._mapName = name;
                    if (slot._id == room._leader)
                    {
                        AllUtils.LogRoomBattleStart(room);
                        room._state = RoomState.Rendezvous;
                        room.updateRoomInfo();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.info("BATTLE_LOADING_REC: " + ex.ToString());
            }
        }
    }
}