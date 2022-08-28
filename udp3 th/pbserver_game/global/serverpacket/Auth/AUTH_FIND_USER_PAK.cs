using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class AUTH_FIND_USER_PAK : SendPacket
    {
        private uint _erro;
        private Account player;
        public AUTH_FIND_USER_PAK(uint erro, Account player)
        {
            _erro = erro;
            this.player = player;
        }

        public override void write()
        {
            writeH(298);
            writeD(_erro);
            if (_erro == 0)
            {
                writeD(player._rank);
                writeD(ComDiv.GetPlayerStatus(player._status, player._isOnline));
                writeS(ClanManager.getClan(player.clanId)._name, 17);
                writeD(player._statistic.fights);
                writeD(player._statistic.fights_win);
                writeD(player._statistic.fights_lost);
                writeD(player._statistic.fights_draw);
                writeD(player._statistic.kills_count);
                writeD(player._statistic.headshots_count);
                writeD(player._statistic.deaths_count);
                writeD(player._statistic.totalfights_count);
                writeD(player._statistic.totalkills_count);
                writeD(player._statistic.escapes);
                writeD(player._statistic.fights);
                writeD(player._statistic.fights_win);
                writeD(player._statistic.fights_lost);
                writeD(player._statistic.fights_draw);
                writeD(player._statistic.kills_count);
                writeD(player._statistic.headshots_count);
                writeD(player._statistic.deaths_count);
                writeD(player._statistic.totalfights_count);
                writeD(player._statistic.totalkills_count);
                writeD(player._statistic.escapes);
                writeD(player._equip._primary);
                writeD(player._equip._secondary);
                writeD(player._equip._melee);
                writeD(player._equip._grenade);
                writeD(player._equip._special);
                writeD(player._equip._red);
                writeD(player._equip._blue);
                writeD(player._equip._helmet);
                writeD(player._equip._beret);
                writeD(player._equip._dino);
                writeH(0);
                writeC(0);
            }
        }
    }
}