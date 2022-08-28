using Core;
using Core.models.enums;
using Core.models.room;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class ROOM_CLOSE_SLOT_REC : ReceiveGamePacket
    {
        private int slotInfo;
        private uint erro;
        public ROOM_CLOSE_SLOT_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            slotInfo = readD(); //0x10000000 = Abrindo slot fechado || 0 = Fechando slot
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                Room room = p == null ? null : p._room;
                if (room != null && room._leader == p._slotId)
                {
                    SLOT slot = room.getSlot(slotInfo & 0xFFFFFFF);
                    if (slot == null)
                        return;
                    if ((slotInfo & 0x10000000) == 0x10000000)
                        OpenSlot(room, slot);
                    else
                        CloseSlot(room, slot);
                }
                else erro = 2147484673;
                _client.SendPacket(new ROOM_CLOSE_SLOT_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.warning("[ROOM_CLOSE_SLOT_REC] " + ex.ToString());
            }
        }
        private void CloseSlot(Room room, SLOT slot)
        {
            switch (slot.state)
            {
                case SLOT_STATE.EMPTY:
                    room.changeSlotState(slot, SLOT_STATE.CLOSE, true);
                    break;
                case SLOT_STATE.CLAN:
                case SLOT_STATE.NORMAL:
                case SLOT_STATE.INFO:
                case SLOT_STATE.INVENTORY:
                case SLOT_STATE.OUTPOST:
                case SLOT_STATE.SHOP:
                case SLOT_STATE.READY:
                    Account player = room.getPlayerBySlot(slot);
                    if (player != null && !player.AntiKickGM)
                    {
                        if (((int)slot.state != 8 && (room._channelType == 4 && (int)room._state != 1 || room._channelType != 4) ||
                            (int)slot.state == 8 && (room._channelType == 4 && (int)room._state == 0 || room._channelType != 4)))
                        {
                            player.SendPacket(new SERVER_MESSAGE_KICK_PLAYER_PAK()); //2147484673 - 4vs4 error
                            room.RemovePlayer(player, slot, false);
                        }
                    }
                    break;
            }
        }
        private void OpenSlot(Room room, SLOT slot)
        {
            if (((slotInfo & 0x10000000) != 0x10000000) || slot.state != SLOT_STATE.CLOSE)
                return;
            room.changeSlotState(slot, SLOT_STATE.EMPTY, true);
        }
    }
}