using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_MEMBER_INFO_UPDATE_PAK : SendPacket
    {
        private Account p;
        private ulong status;
        public CLAN_MEMBER_INFO_UPDATE_PAK(Account pl)
        {
            p = pl;
            status = ComDiv.GetClanStatus(pl._status, pl._isOnline);
        }

        public override void write()
        {
            writeH(1380);
            writeQ(p.player_id);
            writeS(p.player_name, 33);
            writeC((byte)p._rank);
            writeC((byte)p.clanAccess);
            writeQ(status);
            writeD(p.clanDate);
            writeC((byte)p.name_color);
        }
    }
}