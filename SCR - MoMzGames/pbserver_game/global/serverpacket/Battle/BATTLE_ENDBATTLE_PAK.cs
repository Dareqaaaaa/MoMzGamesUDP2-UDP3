using Core.models.account.clan;
using Core.models.enums;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;

namespace Game.global.serverpacket
{
    public class BATTLE_ENDBATTLE_PAK : SendPacket
    {
        private Room r;
        private Account p;
        private TeamResultType winner = TeamResultType.TeamDraw;
        private ushort playersFlag, missionsFlag;
        private bool isBotMode;
        private byte[] array1;
        public BATTLE_ENDBATTLE_PAK(Account p)
        {
            this.p = p;
            if (p != null)
            {
                r = p._room;
                winner = AllUtils.GetWinnerTeam(r);
                isBotMode = r.isBotMode();
                AllUtils.getBattleResult(r, out missionsFlag, out playersFlag, out array1);
            }
        }
        public BATTLE_ENDBATTLE_PAK(Account p, TeamResultType winner, ushort playersFlag, ushort missionsFlag, bool isBotMode, byte[] a1)
        {
            this.p = p;
            this.winner = winner;
            this.playersFlag = playersFlag;
            this.missionsFlag = missionsFlag;
            this.isBotMode = isBotMode;
            array1 = a1;
            if (p != null)
                r = p._room;
        }
        public override void write()
        {
            if (p == null || r == null)
                return;
            writeH(3336);
            writeC((byte)winner);
            writeH(playersFlag);
            writeH(missionsFlag);
            writeB(array1);
            Clan clan = ClanManager.getClan(p.clanId);
            writeS(p.player_name, 33);
            writeD(p._exp);
            writeD(p._rank);
            writeD(p._rank);
            writeD(p._gp);
            writeD(p._money);
            writeD(clan._id);
            writeD(p.clanAccess);
            writeD(0);
            writeD(0);
            writeC((byte)p.pc_cafe);
            writeC((byte)p.tourneyLevel);
            writeC((byte)p.name_color);
            writeS(clan._name, 17);
            writeC((byte)clan._rank);
            writeC((byte)clan.getClanUnit());
            writeD(clan._logo);
            writeC((byte)clan._name_color);
            writeD(0);
            writeC(0);
            writeD(0);
            writeD(p.LastRankUpDate);
            writeD(p._statistic.fights);
            writeD(p._statistic.fights_win);
            writeD(p._statistic.fights_lost);
            writeD(p._statistic.fights_draw);
            writeD(p._statistic.kills_count);
            writeD(p._statistic.headshots_count);
            writeD(p._statistic.deaths_count);
            writeD(p._statistic.totalfights_count);
            writeD(p._statistic.totalkills_count);
            writeD(p._statistic.escapes);
            writeD(p._statistic.fights);
            writeD(p._statistic.fights_win);
            writeD(p._statistic.fights_lost);
            writeD(p._statistic.fights_draw);
            writeD(p._statistic.kills_count);
            writeD(p._statistic.headshots_count);
            writeD(p._statistic.deaths_count);
            writeD(p._statistic.totalfights_count);
            writeD(p._statistic.totalkills_count);
            writeD(p._statistic.escapes);
            if (isBotMode)
            {
                for (int i = 0; i < 16; i++)
                    writeH((ushort)r._slots[i].Score);
            }
            else if (r.room_type == 2 || r.room_type == 4 || r.room_type == 7 || r.room_type == 12) //3 sabo || 4 defe
            {
                writeH((ushort)(r.room_type == 7 ? r.red_dino : (r.room_type == 12 ? r._redKills : r.red_rounds)));
                writeH((ushort)(r.room_type == 7 ? r.blue_dino : (r.room_type == 12 ? r._blueKills : r.blue_rounds)));
                for (int i = 0; i < 16; i++)
                    writeC((byte)r._slots[i].objetivos);
            }
            writeC(0);
            writeD(0);
            writeB(new byte[16]);
        }
    }
}