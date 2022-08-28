using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class BATTLE_ROOM_INFO_PAK : SendPacket
    {
        private Room room;
        private bool isBotMode;
        public BATTLE_ROOM_INFO_PAK(Room r)
        {
            room = r;
            if (room != null)
                isBotMode = room.isBotMode();
        }
        public BATTLE_ROOM_INFO_PAK(Room r, bool isBotMode)
        {
            room = r;
            this.isBotMode = isBotMode;
        }
        public override void write()
        {
            if (room == null)
                return;
            writeH(3848);
            writeD(room._roomId);
            writeS(room.name, 23);
            writeH((short)room.mapId);
            writeC(room.stage4v4);
            writeC(room.room_type);
            writeC((byte)room._state);
            writeC((byte)room.getAllPlayers().Count);
            writeC((byte)room.getSlotCount());
            writeC((byte)room._ping);
            writeC(room.weaponsFlag);
            writeC(room.random_map);
            writeC(room.special);
            Account leader = room.getLeader();
            writeS(leader != null ? leader.player_name : "", 33);
            writeD(room.killtime);
            writeC(room.limit);
            writeC(room.seeConf);
            writeH((short)room.autobalans);
            if (isBotMode)
            {
                writeC(room.aiCount);
                writeC(room.aiLevel);
            }
        }
    }
}