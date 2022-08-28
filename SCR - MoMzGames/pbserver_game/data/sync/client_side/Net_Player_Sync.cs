using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.data.sync.client_side
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
            Account player = AccountManager.getAccount(playerId, true);
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