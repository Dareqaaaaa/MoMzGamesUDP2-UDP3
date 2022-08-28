using Core.models.enums.friends;
using Core.server;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_MEMBER_INFO_CHANGE_PAK : SendPacket
    {
        private Account p;
        private ulong status;
        public CLAN_MEMBER_INFO_CHANGE_PAK(Account player)
        {
            p = player;
            status = ComDiv.GetClanStatus(player._status, player._isOnline);
        }
        public CLAN_MEMBER_INFO_CHANGE_PAK(Account player, FriendState st)
        {
            p = player;
            if (st == 0)
                status = ComDiv.GetClanStatus(player._status, player._isOnline);
            else
                status = ComDiv.GetClanStatus(st);
        }
        public override void write()
        {
            writeH(1355);
            writeQ(p.player_id);
            writeQ(status);
        }
    }
}