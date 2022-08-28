using Auth.data.model;
using Core.models.enums.friends;
using Core.server;

namespace Auth.global.serverpacket
{
    public class CLAN_MEMBER_INFO_CHANGE_PAK : SendPacket
    {
        private ulong status;
        private Account member;
        public CLAN_MEMBER_INFO_CHANGE_PAK(Account player)
        {
            member = player;
            status = ComDiv.GetClanStatus(player._status, player._isOnline);
        }
        public CLAN_MEMBER_INFO_CHANGE_PAK(Account player, FriendState st)
        {
            member = player;
            if (st == 0)
                status = ComDiv.GetClanStatus(player._status, player._isOnline);
            else
                status = ComDiv.GetClanStatus(st);
        }
        public override void write()
        {
            writeH(1355);
            writeQ(member.player_id);
            writeQ(status);
        }
    }
}