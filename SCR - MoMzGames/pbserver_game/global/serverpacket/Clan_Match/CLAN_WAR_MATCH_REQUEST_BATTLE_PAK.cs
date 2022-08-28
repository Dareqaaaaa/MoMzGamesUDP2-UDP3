﻿using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_MATCH_REQUEST_BATTLE_PAK : SendPacket
    {
        public Match mt;
        public Account p;
        public CLAN_WAR_MATCH_REQUEST_BATTLE_PAK(Match match, Account p)
        {
            this.mt = match;
            this.p = p;
        }

        public override void write()
        {
            writeH(1555);
            writeH((short)mt._matchId);
            writeH((ushort)mt.getServerInfo());
            writeH((ushort)mt.getServerInfo());
            writeC((byte)mt._state);
            writeC((byte)mt.friendId);
            writeC((byte)mt.formação);
            writeC((byte)mt.getCountPlayers());
            writeD(mt._leader);
            writeC(0);
            writeD(mt.clan._id);
            writeC((byte)mt.clan._rank);
            writeD(mt.clan._logo);
            writeS(mt.clan._name, 17);
            writeT(mt.clan._pontos);
            writeC((byte)mt.clan._name_color);
            if (p != null)
            {
                writeC((byte)p._rank);
                writeS(p.player_name, 33);
                writeQ(p.player_id);
                writeC((byte)mt._slots[mt._leader].state);
            }
            else
                writeB(new byte[43]);
        }
    }
}