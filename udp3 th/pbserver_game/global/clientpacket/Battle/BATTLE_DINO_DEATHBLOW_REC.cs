using Core;
using Core.models.enums;
using Core.models.enums.missions;
using Core.models.room;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class BATTLE_DINO_DEATHBLOW_REC : ReceiveGamePacket
    {
        private int weaponId, TRex;
        public BATTLE_DINO_DEATHBLOW_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            TRex = readC();
            weaponId = readD();
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null)
                    return;
                Room room = p._room;
                if (room != null && room.round.Timer == null && room._state == RoomState.Battle &&
                    room.TRex == TRex)
                {
                    SLOT slot = room.getSlot(p._slotId);
                    if (slot == null || slot.state != SLOT_STATE.BATTLE)
                        return;
                    if (slot._team == 0) room.red_dino += 5;
                    else room.blue_dino += 5;
                    using (BATTLE_DINO_PLACAR_PAK packet = new BATTLE_DINO_PLACAR_PAK(room))
                        room.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
                    AllUtils.CompleteMission(room, p, slot, MISSION_TYPE.DEATHBLOW, weaponId);
                }
            }
            catch (Exception ex)
            {
                Logger.info("BATTLE_DINO_DEATHBLOW_REC: " + ex.ToString());
            }
        }
    }
}