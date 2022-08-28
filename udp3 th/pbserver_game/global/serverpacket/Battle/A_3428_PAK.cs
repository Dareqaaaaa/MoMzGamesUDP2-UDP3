using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class A_3428_PAK : SendPacket
    {
        private Room room;
        public A_3428_PAK(Room room)
        {
            this.room = room;
        }

        public override void write()
        {
            writeH(3429);
            writeD(room.room_type);
            int remaining = room.getInBattleTime();
            writeD((room.getTimeByMask() * 60) - remaining);
            if (room.room_type == 7)
            {
                writeD(room.red_dino);
                writeD(room.blue_dino);
            }
            else if (room.room_type == 1 || room.room_type == 8 || room.room_type == 13)
            {
                writeD(room._redKills);
                writeD(room._blueKills);
            }
            else
            {
                writeD(room.red_rounds);
                writeD(room.blue_rounds);
            }
        }
    }
}