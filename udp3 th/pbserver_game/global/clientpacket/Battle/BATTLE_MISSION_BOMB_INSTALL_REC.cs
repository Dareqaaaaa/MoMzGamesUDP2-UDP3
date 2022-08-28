using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.data.sync.client_side;
using System;

namespace Game.global.clientpacket
{
    public class BATTLE_MISSION_BOMB_INSTALL_REC : ReceiveGamePacket
    {
        private int slotIdx;
        private float x, y, z;
        private byte area;
        public BATTLE_MISSION_BOMB_INSTALL_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            slotIdx = readD();
            area = readC();
            x = readT();
            y = readT();
            z = readT();
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                Room room = player == null ? null : player._room;
                if (room != null && room.round.Timer == null && room._state == RoomState.Battle && !room.C4_actived)
                {
                    SLOT slot = room.getSlot(slotIdx);
                    if (slot == null || slot.state != SLOT_STATE.BATTLE)
                        return;
                    Net_Room_C4.InstallBomb(room, slot, area, x, y, z);
                }
            }
            catch (Exception ex)
            {
                Logger.info("[BATTLE_MISSION_BOMB_INSTALL_REC]: " + ex.ToString());
            }
        }
    }
}