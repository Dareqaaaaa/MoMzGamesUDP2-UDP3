using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_MEMBER_INFO_INSERT_PAK : SendPacket
    {
        private Account p;
        private ulong status;
        public CLAN_MEMBER_INFO_INSERT_PAK(Account pl)
        {
            p = pl;
            status = ComDiv.GetClanStatus(pl._status, pl._isOnline);
        }

        public override void write()
        {
            writeH(1351);
            writeC((byte)(p.player_name.Length + 1));
            writeS(p.player_name, p.player_name.Length + 1);
            writeQ(p.player_id);
            writeQ(status);
            writeC((byte)p._rank);
        }
    }
}