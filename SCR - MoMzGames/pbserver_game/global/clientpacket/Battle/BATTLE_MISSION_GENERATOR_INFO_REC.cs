using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.data.sync.client_side;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    public class BATTLE_MISSION_GENERATOR_INFO_REC : ReceiveGamePacket
    {
        private ushort barRed, barBlue;
        private List<ushort> damages = new List<ushort>();
        public BATTLE_MISSION_GENERATOR_INFO_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }
        public override void read()
        {
            barRed = readUH();
            barBlue = readUH();
            for (int i = 0; i < 16; i++)
                damages.Add(readUH());
        }
        public override void run()
        {
            try
            {
                Account player = _client._player;
                Room room = player == null ? null : player._room;
                if (room != null && room.round.Timer == null && room._state == RoomState.Battle && !room.swapRound)
                {
                    SLOT slot = room.getSlot(player._slotId);
                    if (slot == null || slot.state != SLOT_STATE.BATTLE)
                        return;
                    room.Bar1 = barRed;
                    room.Bar2 = barBlue;
                    for (int i = 0; i < 16; i++)
                    {
                        SLOT slotR = room._slots[i];
                        if (slotR._playerId > 0 && (int)slotR.state == 13)
                        {
                            slotR.damageBar1 = damages[i];
                            slotR.earnedXP = damages[i] / 600;
                        }
                    }
                    using (BATTLE_MISSION_GENERATOR_INFO_PAK packet = new BATTLE_MISSION_GENERATOR_INFO_PAK(room))
                        room.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
                    if (barRed == 0) Net_Room_Sabotage_Sync.EndRound(room, 1);
                    else if (barBlue == 0) Net_Room_Sabotage_Sync.EndRound(room, 0);
                }
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}