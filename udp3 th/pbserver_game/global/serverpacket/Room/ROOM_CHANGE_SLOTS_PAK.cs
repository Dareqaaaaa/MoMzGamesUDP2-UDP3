using Core.models.room;
using Core.server;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class ROOM_CHANGE_SLOTS_PAK : SendPacket
    {
        private int _type, _leader;
        private List<SLOT_CHANGE> _slots;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="leader"></param>
        /// <param name="type">0=Normal; 1=Slots ajustados por balanceamento; 2=Dono da sala trocou os times</param>
        public ROOM_CHANGE_SLOTS_PAK(List<SLOT_CHANGE> slots, int leader, int type)
        {
            _slots = slots;
            _leader = leader;
            _type = type;
        }

        public override void write()
        {
            writeH(3877);
            writeC((byte)_type);
            writeC((byte)_leader);
            writeC((byte)_slots.Count);
            for (int i = 0; i < _slots.Count; i++)
            {
                SLOT_CHANGE slot = _slots[i];
                writeC((byte)slot.oldSlot._id);
                writeC((byte)slot.newSlot._id);
                writeC((byte)slot.oldSlot.state);
                writeC((byte)slot.newSlot.state);
            }
        }
    }
}