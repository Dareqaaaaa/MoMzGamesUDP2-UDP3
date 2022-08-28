using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.data.sync.client_side
{
    public static class Net_Clan_Sync
    {
        public static void Load(ReceiveGPacket p)
        {
            long playerId = p.readQ();
            int type = p.readC();
            Account player = AccountManager.getAccount(playerId, true);
            if (player == null)
                return;

            if (type == 3)
            {
                int clanId = p.readD();
                int clanAccess = p.readC();
                player.clanId = clanId;
                player.clanAccess = clanAccess;
            }
        }
    }
}