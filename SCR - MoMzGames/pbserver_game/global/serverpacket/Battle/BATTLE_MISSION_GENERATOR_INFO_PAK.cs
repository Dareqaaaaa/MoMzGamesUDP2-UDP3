using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BATTLE_MISSION_GENERATOR_INFO_PAK : SendPacket
    {
        private Room _room;
        public BATTLE_MISSION_GENERATOR_INFO_PAK(Room room)
        {
            _room = room;
        }

        public override void write()
        {
            writeH(3369);
            writeH((ushort)_room.Bar1);
            writeH((ushort)_room.Bar2);
            for (int i = 0; i < 16; i++)
                writeH(_room._slots[i].damageBar1);
            //600 - 5+
        }
    }
}