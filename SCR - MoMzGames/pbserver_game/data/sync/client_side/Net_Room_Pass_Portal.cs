using Core;
using Core.models.enums;
using Core.models.enums.missions;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.utils;
using Game.data.xml;
using Game.global.serverpacket;
using System;

namespace Game.data.sync.client_side
{
    public static class Net_Room_Pass_Portal
    {
        public static void Load(ReceiveGPacket p)
        {
            int roomId = p.readH();
            int channelId = p.readH();
            int slotId = p.readC(); //player
            int portalId = p.readC(); //portal
            Channel ch = ChannelsXML.getChannel(channelId);
            if (ch == null)
                return;
            Room room = ch.getRoom(roomId);
            if (room != null && room.round.Timer == null && room._state == RoomState.Battle && room.room_type == 7)
            {
                SLOT slot = room.getSlot(slotId);
                if (slot != null && slot.state == SLOT_STATE.BATTLE)
                {
                    ++slot.passSequence;
                    if (slot._team == 0) room.red_dino += 5;
                    else room.blue_dino += 5;
                    CompleteMission(room, slot);
                    using (BATTLE_MISSION_ESCAPE_PAK packet = new BATTLE_MISSION_ESCAPE_PAK(room, slot))
                    using (BATTLE_DINO_PLACAR_PAK packet2 = new BATTLE_DINO_PLACAR_PAK(room))
                        room.SendPacketToPlayers(packet, packet2, SLOT_STATE.BATTLE, 0);
                }
            }
            if (p.getBuffer().Length > 8)
                Logger.warning("[Invalid PORTAL: " + BitConverter.ToString(p.getBuffer()) + "]");
        }
        private static void CompleteMission(Room room, SLOT slot)
        {
            MISSION_TYPE mission = MISSION_TYPE.NA;
            if (slot.passSequence == 1) mission = MISSION_TYPE.TOUCHDOWN;
            else if (slot.passSequence == 2) mission = MISSION_TYPE.TOUCHDOWN_ACE_ATTACKER;
            else if (slot.passSequence == 3) mission = MISSION_TYPE.TOUCHDOWN_HATTRICK;
            else if (slot.passSequence >= 4) mission = MISSION_TYPE.TOUCHDOWN_GAME_MAKER;
            if (mission != MISSION_TYPE.NA)
                AllUtils.CompleteMission(room, slot, mission, 0);
        }
    }
}