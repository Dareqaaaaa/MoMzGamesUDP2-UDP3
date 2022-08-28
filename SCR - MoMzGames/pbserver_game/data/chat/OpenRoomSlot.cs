﻿using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;

namespace Game.data.chat
{
    public static class OpenRoomSlot
    {
        public static string OpenSpecificSlot(string str, Account player, Room room)
        {
            int slotId = int.Parse(str.Substring(6));
            if (slotId < 1 || slotId > 16)
                return Translation.GetLabel("OpenRoomSlot_WrongValue");
            slotId--;
            if (player != null && room != null)
            {
                SLOT slot = room.getSlot(slotId);
                if (slot != null && (int)slot.state == 1)
                {
                    slot.state = SLOT_STATE.EMPTY;
                    room.updateSlotsInfo();
                    return Translation.GetLabel("OpenRoomSlot_Success1", slotId);
                }
                else
                    return Translation.GetLabel("OpenRoomSlot_Fail1");
            }
            else
                return Translation.GetLabel("OpenRoomSlot_Fail2");
        }
        public static string OpenRandomSlot(string str, Account player)
        {
            int roomId = int.Parse(str.Substring(6));
            if (roomId-- <= 0)
                return Translation.GetLabel("OpenRoomSlot_WrongValue2");
            if (player == null)
                return Translation.GetLabel("OpenRoomSlot_Fail6");
            Channel channel = player.getChannel();
            if (channel == null)
                return Translation.GetLabel("GeneralChannelInvalid");
            Room rm = channel.getRoom(roomId);
            if (rm != null)
            {
                bool ok = false;
                for (int i = 0; i < 16; i++)
                {
                    SLOT slot = rm._slots[i];
                    if ((int)slot.state == 1)
                    {
                        slot.state = SLOT_STATE.EMPTY;
                        ok = true;
                        break;
                    }
                }
                if (ok)
                    rm.updateSlotsInfo();
                return !ok ? Translation.GetLabel("OpenRoomSlot_Fail3") : Translation.GetLabel("OpenRoomSlot_Success2");
            }
            else
                return Translation.GetLabel("GeneralRoomNotFounded");
        }
        public static string OpenAllSlots(string str, Account player)
        {
            int roomId = int.Parse(str.Substring(6));
            if (roomId-- <= 0)
                return Translation.GetLabel("OpenRoomSlot_WrongValue2");
            if (player == null)
                return Translation.GetLabel("OpenRoomSlot_Fail6");
            Channel channel = player.getChannel();
            if (channel == null)
                return Translation.GetLabel("OpenRoomSlot_Fail5");
            Room rm = channel.getRoom(roomId);
            if (rm != null)
            {
                for (int i = 0; i < 16; i++)
                {
                    SLOT slot = rm._slots[i];
                    if ((int)slot.state == 1)
                        slot.state = SLOT_STATE.EMPTY;
                }
                rm.updateSlotsInfo();
                return Translation.GetLabel("OpenRoomSlot_Success3");
            }
            else
                return Translation.GetLabel("GeneralRoomNotFounded");
        }
    }
}