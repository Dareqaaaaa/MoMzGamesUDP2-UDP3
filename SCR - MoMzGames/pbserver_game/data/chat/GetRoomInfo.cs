using Core;
using Core.models.room;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.data.chat
{
    public static class GetRoomInfo
    {
        public static string GetSlotStats(string str, Account player, Room room)
        {
            int slotIdx = (int.Parse(str.Substring(5)) - 1);
            string infos = "Informações:";
            if (room != null)
            {
                SLOT slot = room.getSlot(slotIdx);
                if (slot != null)
                {
                    infos += "\nIndex: " + slot._id;
                    infos += "\nTeam: " + slot._team;
                    infos += "\nFlag: " + slot._flag;
                    infos += "\nAccountId: " + slot._playerId;
                    infos += "\nState: " + slot.state;
                    infos += "\nMissions: " + ((slot.Missions != null) ? "Valido" : "Null");
                    player.SendPacket(new SERVER_MESSAGE_ANNOUNCE_PAK(infos));
                    return "Logs do slot geradas com sucesso. [Servidor]";
                }
                else
                    return "Slot inválido. [Servidor]";
            }
            else
                return "Sala inválida. [Servidor]";
        }
    }
}