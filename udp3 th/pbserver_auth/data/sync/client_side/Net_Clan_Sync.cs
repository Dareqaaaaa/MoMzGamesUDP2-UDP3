using Auth.data.managers;
using Auth.data.model;
using Auth.data.sync.update;
using Core.server;

namespace Auth.data.sync.client_side
{
    public static class Net_Clan_Sync
    {
        public static void Load(ReceiveGPacket p)
        {
            long playerId = p.readQ(),
                memberId;
            int type = p.readC();
            Account player = AccountManager.getInstance().getAccount(playerId, true);
            if (player == null)
                return;

            if (type == 0)
                ClanInfo.ClearList(player);
            else if (type == 1)
            {
                memberId = p.readQ();
                string name = p.readS(p.readC());
                byte[] status = p.readB(4);
                byte rank = p.readC();
                Account member = new Account
                {
                    player_id = memberId,
                    player_name = name,
                    _rank = rank
                };
                member._status.SetData(status, memberId);
                ClanInfo.AddMember(player, member);
            }
            else if (type == 2)
            {
                memberId = p.readQ();
                ClanInfo.RemoveMember(player, memberId);
            }
            else if (type == 3)
            {
                int clanId = p.readD();
                int clanAccess = p.readC();
                player.clan_id = clanId;
                player.clanAccess = clanAccess;
            }
        }
    }
}