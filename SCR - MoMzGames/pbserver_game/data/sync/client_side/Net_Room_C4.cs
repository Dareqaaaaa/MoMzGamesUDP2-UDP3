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
    public static class Net_Room_C4
    {
        public static void Load(ReceiveGPacket p)
        {
            int roomId = p.readH();
            int channelId = p.readH();
            int type = p.readC();
            int slotIdx = p.readC();
            int areaId = 0;
            float x = 0, y = 0, z = 0;
            if (type == 0)
            {
                areaId = p.readC();
                x = p.readT();
                y = p.readT();
                z = p.readT();
                if (p.getBuffer().Length > 21)
                    Logger.warning("[Invalid BOMB0: " + BitConverter.ToString(p.getBuffer()) + "]");
            }
            else if (type == 1)
                if (p.getBuffer().Length > 8)
                    Logger.warning("[Invalid BOMB1: " + BitConverter.ToString(p.getBuffer()) + "]");
            Channel ch = ChannelsXML.getChannel(channelId);
            if (ch == null)
                return;
            Room room = ch.getRoom(roomId);
            if (room != null && room.round.Timer == null && room._state == RoomState.Battle)
            {
                SLOT slot = room.getSlot(slotIdx);
                if (slot == null || slot.state != SLOT_STATE.BATTLE)
                    return;
                if (type == 0)
                    InstallBomb(room, slot, areaId, x, y, z);
                else if (type == 1)
                    UninstallBomb(room, slot);
            }
        }
        public static void InstallBomb(Room room, SLOT slot, int areaId, float x, float y, float z)
        {
            if (room.C4_actived)
                return;
            using (BATTLE_MISSION_BOMB_INSTALL_PAK packet = new BATTLE_MISSION_BOMB_INSTALL_PAK(slot._id, (byte)areaId, x, y, z))
                room.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
            if (room.room_type != 10)
            {
                room.C4_actived = true;
                slot.objetivos++;
                AllUtils.CompleteMission(room, slot, MISSION_TYPE.C4_PLANT, 0);
                room.StartBomb();
            }
        }
        public static void UninstallBomb(Room room, SLOT slot)
        {
            if (!room.C4_actived)
                return;
            using (BATTLE_MISSION_BOMB_UNINSTALL_PAK packet = new BATTLE_MISSION_BOMB_UNINSTALL_PAK(slot._id))
                room.SendPacketToPlayers(packet, SLOT_STATE.BATTLE, 0);
            if (room.room_type != 10)
            {
                slot.objetivos++;
                room.blue_rounds++;
                AllUtils.CompleteMission(room, slot, MISSION_TYPE.C4_DEFUSE, 0);
                AllUtils.BattleEndRound(room, 1, RoundEndType.Uninstall);
            }
        }
    }
}