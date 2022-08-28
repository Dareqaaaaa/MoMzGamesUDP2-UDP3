using Core.models.account.players;
using Core.server;

namespace Game.global.serverpacket
{
    public class BASE_USER_CHANGE_STATS_PAK : SendPacket
    {
        private PlayerStats s;
        public BASE_USER_CHANGE_STATS_PAK(PlayerStats s)
        {
            this.s = s;
        }

        public override void write()
        {
            writeH(2610);
            writeD(s.fights);
            writeD(s.fights_win);
            writeD(s.fights_lost);
            writeD(s.fights_draw);
            writeD(s.kills_count);
            writeD(s.headshots_count);
            writeD(s.deaths_count);
            writeD(s.totalfights_count);
            writeD(s.totalkills_count);
            writeD(s.escapes);
            writeD(s.fights);
            writeD(s.fights_win);
            writeD(s.fights_lost);
            writeD(s.fights_draw);
            writeD(s.kills_count);
            writeD(s.headshots_count);
            writeD(s.deaths_count);
            writeD(s.totalfights_count);
            writeD(s.totalkills_count);
            writeD(s.escapes);
        }
    }
}