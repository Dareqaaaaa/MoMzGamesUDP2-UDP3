using Auth.data.managers;
using Auth.data.model;
using Core.server;

namespace Auth.data.sync.client_side
{
    public static class Net_Player_Sync
    {
        public static void Load(ReceiveGPacket p)
        {
            long playerId = p.readQ();
            int type = p.readC();
            int rank = p.readC();
            int gold = p.readD();
            int cash = p.readD();
            Account player = AccountManager.getInstance().getAccount(playerId, true);
            if (player == null)
                return;

            if (type == 0)
            {
                player._rank = rank;
                player._gp = gold;
                player._money = cash;
            }
        }
    }
}