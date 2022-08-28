using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using System;

namespace Game.global.clientpacket
{
    public class ROOM_CHANGE_INFO_REC : ReceiveGamePacket
    {
        public ROOM_CHANGE_INFO_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            try
            {
                Account player = _client._player;
                Room room = player == null ? null : player._room;
                if (room != null && room._leader == player._slotId && room._state == RoomState.Ready)
                {
                    readD();
                    room.name = readS(23);
                    room.mapId = readH();
                    room.stage4v4 = readC();
                    byte stageType = readC();
                    if (stageType != room.room_type)
                    {
                        room.room_type = stageType;
                        int count = 0;
                        for (int i = 0; i < 16; i++)
                        {
                            SLOT slot = room._slots[i];
                            if ((int)slot.state == 8)
                            {
                                slot.state = SLOT_STATE.NORMAL;
                                count++;
                            }
                        }
                        if (count > 0)
                            room.updateSlotsInfo();
                    }
                    readC();
                    readC();
                    readC();
                    room._ping = readC();
                    room.weaponsFlag = readC();
                    room.random_map = readC();
                    room.special = readC();
                    readS(33);
                    room.killtime = readC();
                    readC();
                    readC();
                    readC();
                    room.limit = readC();
                    room.seeConf = readC();
                    room.autobalans = readH();
                    room.aiCount = readC();
                    room.aiLevel = readC();
                    room.updateRoomInfo();
                }
            }
            catch (Exception ex)
            {
                Logger.info("ROOM_CHANGE_INFO_REC: " + ex.ToString());
            }
        }

        public override void run()
        {
        }
    }
}