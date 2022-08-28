using Core;
using Core.server;
using Game.data.model;
using System;

namespace Game.global.serverpacket
{
    public class LOBBY_GET_ROOMINFO_PAK : SendPacket
    {
        private Room room;
        private Account leader;
        public LOBBY_GET_ROOMINFO_PAK(Room room, Account leader)
        {
            this.room = room;
            this.leader = leader;
        }

        public override void write()
        {
            if (room == null || leader == null)
                return;
            writeH(3088);
            try
            {
                writeS(leader.player_name, 33);
                writeC((byte)room.killtime);
                writeC((byte)(room.rodada - 1));
                writeH((ushort)room.getInBattleTime());
                writeC((byte)room.limit);
                writeC((byte)room.seeConf);
                writeH((ushort)room.autobalans);
            }
            catch (Exception ex)
            {
                writeS("", 33);
                writeB(new byte[8]);
                Logger.warning("[LOBBY_GET_ROOMINFO_PAK] " + ex.ToString());
            }
        }
    }
}