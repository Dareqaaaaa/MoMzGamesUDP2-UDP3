using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;

namespace Game.data.chat
{
    public static class LatencyAnalyze
    {
        public static string StartAnalyze(Account player, Room room)
        {
            if (room == null)
                return Translation.GetLabel("GeneralRoomInvalid");

            SLOT slot = room.getSlot(player._slotId);
            if (slot.state == SLOT_STATE.BATTLE)
            {
                player.DebugPing = !player.DebugPing;
                if (player.DebugPing)
                    return Translation.GetLabel("LatencyInfoOn");
                else
                    return Translation.GetLabel("LatencyInfoOff");
            }
            else
                return Translation.GetLabel("LatencyInfoError");
        }
    }
}