using Core.models.enums;
using Core.server;
using Game.data.model;

namespace Game.data.sync.server_side
{
    public class BATTLE_LEAVE_SYNC
    {
        public static void SendUDPPlayerLeave(Room room, int slotId)
        {
            if (room == null)
                return;
            int count = room.getPlayingPlayers(2, SLOT_STATE.BATTLE, 0, slotId);
            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(2);
                pk.writeD(room.UniqueRoomId);
                pk.writeD((room.mapId * 16) + room.room_type);
                pk.writeC((byte)slotId);
                pk.writeC((byte)count);
                Game_SyncNet.SendPacket(pk.mstream.ToArray(), room.UDPServer.Connection);
            }
        }
    }
}