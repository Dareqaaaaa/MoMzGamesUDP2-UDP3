using Auth.data.managers;
using Auth.data.model;
using Core.models.account;
using Core.models.account.players;
using Core.models.servers;
using Core.server;
using Core.xml;

namespace Auth.data.sync.server_side
{
    public static class SEND_REFRESH_ACC
    {
        public static void RefreshAccount(Account player, bool isConnect)
        {
            Auth_SyncNet.UpdateGSCount(0);
            AccountManager.getInstance().getFriendlyAccounts(player.FriendSystem);
            for (int i = 0; i < player.FriendSystem._friends.Count; i++)
            {
                Friend friend = player.FriendSystem._friends[i];
                PlayerInfo info = friend.player;
                if (info != null)
                {
                    GameServerModel gs = ServersXML.getServer(info._status.serverId);
                    if (gs == null)
                        continue;

                    SendRefreshPacket(0, player.player_id, friend.player_id, isConnect, gs);
                }
            }
            if (player.clan_id > 0)
            {
                for (int i = 0; i < player._clanPlayers.Count; i++)
                {
                    Account member = player._clanPlayers[i];
                    if (member != null && member._isOnline)
                    {
                        GameServerModel gs = ServersXML.getServer(member._status.serverId);
                        if (gs == null)
                            continue;

                        SendRefreshPacket(1, player.player_id, member.player_id, isConnect, gs);
                    }
                }
            }
        }
        public static void SendRefreshPacket(int type, long playerId, long memberId, bool isConnect, GameServerModel gs)
        {
            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(11);
                pk.writeC((byte)type);
                pk.writeC(isConnect);
                pk.writeQ(playerId);
                pk.writeQ(memberId);
                Auth_SyncNet.SendPacket(pk.mstream.ToArray(), gs.Connection);
            }
        }
    }
}