using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.utils;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class BATTLE_RESPAWN_PAK : SendPacket
    {
        private SLOT slot;
        private Room room;
        public BATTLE_RESPAWN_PAK(Room r, SLOT slot)
        {
            this.slot = slot; 
            this.room = r;
        }

        public override void write()
        {
            writeH(3338);
            writeD(slot._id);
            writeD(room.spawnsCount++); //total number of all players' respawns
            writeD(++slot.spawnsCount); //total number of current player's respawns
            writeD(slot._equip._primary);
            writeD(slot._equip._secondary);
            writeD(slot._equip._melee);
            writeD(slot._equip._grenade);
            writeD(slot._equip._special);
            writeD(0);
            writeB(new byte[6] { 100, 100, 100, 100, 100, 1 }); //Durabilidade das armas
            writeD(slot._equip._red);
            writeD(slot._equip._blue);
            writeD(slot._equip._helmet);
            writeD(slot._equip._beret);
            writeD(slot._equip._dino);
            if (room.room_type == 7 || room.room_type == 12)
            {
                List<int> pL = AllUtils.getDinossaurs(room, false, slot._id);
                int TRex = pL.Count == 1 || room.room_type == 12 ? 255 : room.TRex;
                writeC((byte)TRex);
                foreach (int slotId in pL)
                    if (slotId != room.TRex && room.room_type == 7 || room.room_type == 12)
                        writeC((byte)slotId);
                int falta = 8 - pL.Count - (TRex == 255 ? 1 : 0);
                for (int i = 0; i < falta; i++)
                    writeC(255);
                writeC(255);
                writeC(255);
            }
        }
    }
}