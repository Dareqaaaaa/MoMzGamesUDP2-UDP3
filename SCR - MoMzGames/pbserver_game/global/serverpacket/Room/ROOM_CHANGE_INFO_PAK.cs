using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class ROOM_CHANGE_INFO_PAK : SendPacket
    {
        private string _leader;
        private Room _room;
        public ROOM_CHANGE_INFO_PAK(Room room, string leader)
        {
            _room = room;
            _leader = leader;
        }

        public override void write()
        {
            writeH(3859);
            writeS(_leader, 33);
            writeD(_room.killtime);
            writeC(_room.limit);
            writeC(_room.seeConf);
            writeH((short)_room.autobalans); //0 - nada | 1 - qnt. | 2 - rank
        }
    }
}