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
    public class BATTLE_MISSION_DEFENCE_INFO_REC : ReceiveGamePacket
    {
        private ushort tanqueA, tanqueB;
        private List<ushort> _damag1 = new List<ushort>(), _damag2 = new List<ushort>();
        public BATTLE_MISSION_DEFENCE_INFO_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }
        public override void read()
        {
            tanqueA = readUH();
            tanqueB = readUH();
            for (int i = 0; i < 16; i++)
                _damag1.Add(readUH());
            for (int i = 0; i < 16; i++)
                _damag2.Add(readUH());
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
                    room.Bar1 = tanqueA;
                    room.Bar2 = tanqueB;
                    for (int i = 0; i < 16; i++)
                    {
                        SLOT slotR = room._slots[i];
                        if (slotR._playerId > 0 && (int)slotR.state == 13)
                        {
                            slotR.damageBar1 = _damag1[i];
                            slotR.damageBar2 = _damag2[i];
                        }
                    }
                    using (BATTLE_MISSION_DEFENCE_INFO_PAK packet = new BATTLE_MISSION_DEFENCE_INFO_PAK(room))
                        room.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
                    if (tanqueA == 0 && tanqueB == 0)
                        Net_Room_Sabotage_Sync.EndRound(room, 0);
                }
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}