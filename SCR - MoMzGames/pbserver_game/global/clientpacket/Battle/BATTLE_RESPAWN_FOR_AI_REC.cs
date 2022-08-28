using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BATTLE_RESPAWN_FOR_AI_REC : ReceiveGamePacket
    {
        private int slotIdx;
        public BATTLE_RESPAWN_FOR_AI_REC(GameClient gc, byte[] data)
        {
            makeme(gc, data);
        }

        public override void read()
        {
            slotIdx = readD();
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null)
                    return;
                Room room = p._room;
                if (room != null && room._state == RoomState.Battle && p._slotId == room._leader)
                {
                    SLOT slot = room.getSlot(slotIdx);
                    slot.aiLevel = room.IngameAiLevel;
                    room.spawnsCount++;
                    using (BATTLE_RESPAWN_FOR_AI_PAK packet = new BATTLE_RESPAWN_FOR_AI_PAK(slotIdx))
                        room.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
                }
            }
            catch (Exception ex)
            {
                Logger.info("BATTLE_RESPAWN_FOR_AI_REC: " + ex.ToString());
            }
        }
    }
}