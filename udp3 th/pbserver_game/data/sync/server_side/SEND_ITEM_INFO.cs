using Core.models.account.players;
using Core.models.servers;
using Core.server;
using Game.data.model;

namespace Game.data.sync.server_side
{
    public class SEND_ITEM_INFO
    {
        public static void LoadItem(Account player, ItemsModel item)
        {
            if (player == null || player._status.serverId == 0)
                return;
            GameServerModel gs = Game_SyncNet.GetServer(player._status);
            if (gs == null)
                return;

            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(18);
                pk.writeQ(player.player_id);
                pk.writeQ(item._objId);
                pk.writeD(item._id);
                pk.writeC((byte)item._equip);
                pk.writeC((byte)item._category);
                pk.writeD(item._count);
                Game_SyncNet.SendPacket(pk.mstream.ToArray(), gs.Connection);
            }
        }
        /// <summary>
        /// Atualiza 'gp', 'money', 'rank'.
        /// </summary>
        /// <param name="player"></param>
        public static void LoadGoldCash(Account player)
        {
            if (player == null)
                return;
            GameServerModel gs = Game_SyncNet.GetServer(player._status);
            if (gs == null)
                return;

            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(19);
                pk.writeQ(player.player_id);
                pk.writeC(0);
                pk.writeC((byte)player._rank);
                pk.writeD(player._gp);
                pk.writeD(player._money);
                Game_SyncNet.SendPacket(pk.mstream.ToArray(), gs.Connection);
            }
        }
    }
}