using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using System;

namespace Game.global.clientpacket
{
    public class BATTLE_ROOM_INFO_REC : ReceiveGamePacket
    {
        public BATTLE_ROOM_INFO_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            try
            {
                Account player = _client._player;
                Room room = player == null ? null : player._room;
                if (room == null || room._state != RoomState.Ready || room._leader != player._slotId)
                    return;
                readD();
                room.name = readS(23);
                room.mapId = readH();
                room.stage4v4 = readC();
                room.room_type = readC();
                readC();
                readC();
                readC();
                room._ping = readC();
                byte weaponsFlag = readC();
                if (weaponsFlag != room.weaponsFlag)
                {
                    room.weaponsFlag = weaponsFlag;
                    for (int i = 0; i < 16; i++)
                    {
                        SLOT slot = room._slots[i];
                        if ((int)slot.state == 8)
                            slot.state = SLOT_STATE.NORMAL;
                    }
                }
                room.random_map = readC();
                room.special = readC();
                room.aiCount = readC();
                room.aiLevel = readC();
                room.updateRoomInfo();
            }
            catch (Exception ex)
            {
                Logger.info("BATTLE_ROOM_INFO_REC: " + ex.ToString());
            }
        }

        public override void run()
        {
        }
    }
}