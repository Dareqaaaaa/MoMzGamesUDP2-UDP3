using Battle.data.models;
using Battle.network;

namespace Battle.data.sync.client_side
{
    public static class RemovePlayerSync
    {
        public static void Load(ReceivePacket p)
        {
            uint UniqueRoomId = p.readUD();
            int gen2 = p.readD();
            int slotId = p.readC();
            int inBattleCount = p.readC();
            Room room = RoomsManager.getRoom(UniqueRoomId, gen2);
            if (room == null)
                return;

            if (inBattleCount == 0)
                RoomsManager.RemoveRoom(UniqueRoomId);
            else
            {
                Player player = room.getPlayer(slotId, false);
                if (player != null)
                    player.ResetAllInfos();
            }
        }
    }
}