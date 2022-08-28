using Core.models.account.players;
using Core.server;

namespace Game.global.serverpacket
{
    public class LOBBY_GET_PLAYERINFO_PAK : SendPacket
    {
        private PlayerStats st;
        public LOBBY_GET_PLAYERINFO_PAK(PlayerStats stats)
        {
            st = stats;
        }

        public override void write()
        {
            writeH(2640);
            if (st != null)
            {
                writeD(st.fights);
                writeD(st.fights_win);
                writeD(st.fights_lost);
                writeD(st.fights_draw);
                writeD(st.kills_count);
                writeD(st.headshots_count);
                writeD(st.deaths_count);
                writeD(st.totalfights_count);
                writeD(st.totalkills_count);
                writeD(st.escapes);
                writeD(st.fights);
                writeD(st.fights_win);
                writeD(st.fights_lost);
                writeD(st.fights_draw);
                writeD(st.kills_count);
                writeD(st.headshots_count);
                writeD(st.deaths_count);
                writeD(st.totalfights_count);
                writeD(st.totalkills_count);
                writeD(st.escapes);
            }
            else
                writeB(new byte[80]);
        }
    }
}