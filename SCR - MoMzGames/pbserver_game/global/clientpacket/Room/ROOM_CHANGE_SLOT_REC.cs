using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.global.clientpacket
{
    public class ROOM_CHANGE_SLOT_REC : ReceiveGamePacket
    {
        private int teamIdx;
        public ROOM_CHANGE_SLOT_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            teamIdx = readD();
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                Room room = player == null ? null : player._room;
                if (teamIdx < 2 && room != null && (player.LastSlotChange == new DateTime() ||
                    (DateTime.Now - player.LastSlotChange).TotalSeconds >= 1.5)
                    && !room.changingSlots)
                {
                    SLOT slot = room.getSlot(player._slotId);
                    if (slot != null && teamIdx != slot._team && slot.state == SLOT_STATE.NORMAL)
                    {
                        player.LastSlotChange = DateTime.Now;
                        Monitor.Enter(room._slots);
                        room.changingSlots = true;

                        List<SLOT_CHANGE> changeList = new List<SLOT_CHANGE>();
                        room.SwitchNewSlot(changeList, player, slot, teamIdx);
                        if (changeList.Count > 0)
                        {
                            using (ROOM_CHANGE_SLOTS_PAK packet = new ROOM_CHANGE_SLOTS_PAK(changeList, room._leader, 0))
                                room.SendPacketToPlayers(packet);
                        }
                        room.changingSlots = false;
                        Monitor.Exit(room._slots);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.warning("[ROOM_CHANGE_SLOT_REC] " + ex.ToString());
            }
        }
    }
}