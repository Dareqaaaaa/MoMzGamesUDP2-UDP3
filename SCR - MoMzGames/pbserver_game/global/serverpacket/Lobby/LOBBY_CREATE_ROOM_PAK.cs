using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class LOBBY_CREATE_ROOM_PAK : SendPacket
    {
        private Account leader;
        private Room room;
        private uint erro;
        public LOBBY_CREATE_ROOM_PAK(uint err, Room r, Account p)
        {
            erro = err;
            room = r;
            leader = p;
        }

        public override void write()
        {
            writeH(3090);
            writeD(erro == 0 ? (uint)room._roomId : erro);
            if (erro == 0)
            {
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
                writeS(leader.player_name, 33);
                writeD(room.killtime);
                writeC(room.limit);
                writeC(room.seeConf);
                writeH((short)room.autobalans);
            }
        }
    }
}