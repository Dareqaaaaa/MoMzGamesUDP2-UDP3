using Core.models.account.clan;
using Core.models.room;
using Core.server;
using Game.data.managers;
using Game.data.model;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class LOBBY_JOIN_ROOM_PAK : SendPacket
    {
        private uint erro;
        private Room room;
        private int slotId;
        private Account leader;
        public LOBBY_JOIN_ROOM_PAK(uint erro, Account player = null, Account leader = null)
        {
            this.erro = erro;
            if (player != null)
            {
                slotId = player._slotId;
                room = player._room;
                this.leader = leader;
            }
        }
        public override void write()
        {
            writeH(3082);
            if (erro == 0 && room != null && leader != null)
            {
                lock (room._slots)
                    WriteData();
            }
            else writeD(erro);
        }
        private void WriteData()
        {
            List<Account> roomPlayers = room.getAllPlayers();
            writeD(room._roomId);
            writeD(slotId);
            writeD(room._roomId);
            writeS(room.name, 23);
            writeH((short)room.mapId);
            writeC(room.stage4v4);
            writeC(room.room_type);
            writeC((byte)room._state);
            writeC((byte)roomPlayers.Count);
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
            writeS(room.password, 4);
            writeC((byte)room.countdown.getTimeLeft());
            writeD(room._leader);
            for (int i = 0; i < 16; ++i)
            {
                SLOT slot = room._slots[i];
                Account player = room.getPlayerBySlot(slot);
                if (player != null)
                {
                    Clan clan = ClanManager.getClan(player.clanId);
                    writeC((byte)slot.state);
                    writeC((byte)player.getRank());
                    writeD(clan._id);
                    writeD(player.clanAccess);
                    writeC((byte)clan._rank);
                    writeD(clan._logo);
                    writeC((byte)player.pc_cafe);
                    writeC((byte)player.tourneyLevel);
                    writeD((uint)player.effects);
                    writeS(clan._name, 17);
                    writeD(0);
                    writeC(31);
                }
                else
                {
                    writeC((byte)slot.state);
                    writeB(new byte[10]);
                    writeD(4294967295);
                    writeB(new byte[28]);
                }
            }
            writeC((byte)roomPlayers.Count);
            for (int i = 0; i < roomPlayers.Count; i++)
            {
                Account ac = roomPlayers[i];
                writeC((byte)ac._slotId);
                writeC((byte)(ac.player_name.Length + 1));
                writeS(ac.player_name, ac.player_name.Length + 1);
                writeC((byte)ac.name_color);
            }
            if (room.isBotMode())
            {
                writeC(room.aiCount);
                writeC(room.aiLevel);
            }
        }
    }
}