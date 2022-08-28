using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class ROOM_GET_PLAYERINFO_PAK : SendPacket
    {
        private Account p;
        public ROOM_GET_PLAYERINFO_PAK(Account player)
        {
            p = player;
        }

        public override void write()
        {
            writeH(3842);
            if (p == null || p._slotId == -1)
            {
                writeD(0x80000000);
                return;
            }
            Clan clan = ClanManager.getClan(p.clanId);
            writeD(p._slotId);
            writeS(p.player_name, 33);
            writeD(p._exp);
            writeD(p.getRank());
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
            writeC(0);
            writeD(0);
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
            writeD(p._equip._red);
            writeD(p._equip._blue);
            writeD(p._equip._helmet);
            writeD(p._equip._beret);
            writeD(p._equip._dino);
            writeD(p._equip._primary);
            writeD(p._equip._secondary);
            writeD(p._equip._melee);
            writeD(p._equip._grenade);
            writeD(p._equip._special);
            writeD(p._titles.Equiped1);
            writeD(p._titles.Equiped2);
            writeD(p._titles.Equiped3);
        }
    }
}