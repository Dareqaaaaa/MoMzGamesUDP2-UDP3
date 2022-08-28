using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.data.sync.client_side;

namespace Game.global.clientpacket
{
    public class BATTLE_MISSION_BOMB_UNINSTALL_REC : ReceiveGamePacket
    {
        private int slotIdx;
        public BATTLE_MISSION_BOMB_UNINSTALL_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            slotIdx = readD();
        }

        public override void run()
        {
            Account player = _client._player;
            Room room = player == null ? null : player._room;
            if (room != null && room.round.Timer == null && room._state == RoomState.Battle && room.C4_actived)
            {
                SLOT slot = room.getSlot(slotIdx);
                if (slot == null || slot.state != SLOT_STATE.BATTLE)
                    return;
                Net_Room_C4.UninstallBomb(room, slot);
            }
        }
    }
}