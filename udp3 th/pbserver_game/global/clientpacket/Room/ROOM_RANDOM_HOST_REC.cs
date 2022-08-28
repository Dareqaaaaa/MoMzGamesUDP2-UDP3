using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.global.serverpacket;
using System;
using System.Collections.Generic;

namespace Game.global.clientpacket
{
    internal class ROOM_RANDOM_HOST_REC : ReceiveGamePacket
    {
        private List<SLOT> slots = new List<SLOT>();
        private uint erro;
        public ROOM_RANDOM_HOST_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                Room room = p == null ? null : p._room;
                if (room != null && room._leader == p._slotId && room._state == RoomState.Ready)
                {
                    lock (room._slots)
                        for (int i = 0; i < 16; i++)
                        {
                            SLOT slot = room._slots[i];
                            if (slot._playerId > 0 && i != room._leader)
                                slots.Add(slot);
                        }
                    if (slots.Count > 0)
                    {
                        int idx = new Random().Next(slots.Count);
                        SLOT result = slots[idx];
                        erro = room.getPlayerBySlot(result) != null ? (uint)result._id : 0x80000000;
                        slots = null;
                    }
                    else erro = 0x80000000;
                }
                else erro = 0x80000000;
                _client.SendPacket(new ROOM_NEW_HOST_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("ROOM_RANDOM_HOST_REC: " + ex.ToString());
            }
        }
    }
}