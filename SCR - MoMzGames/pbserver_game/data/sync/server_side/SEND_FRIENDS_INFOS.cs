using Core.models.account;
using Core.models.servers;
using Core.server;
using Game.data.model;

namespace Game.data.sync.server_side
{
    public class SEND_FRIENDS_INFOS
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">Conta principal</param>
        /// <param name="friend">Amigo que será adicionado/atualizado..</param>
        /// <param name="type">0=Adicionar|1=Atualizar|2=Deletar</param>
        public static void Load(Account player, Friend friend, int type)
        {
            if (player == null)
                return;

            GameServerModel gs = Game_SyncNet.GetServer(player._status);
            if (gs == null)
                return;

            using (SendGPacket pk = new SendGPacket())
            {
                pk.writeH(17);
                pk.writeQ(player.player_id);
                pk.writeC((byte)type);
                pk.writeQ(friend.player_id);

                if (type != 2)
                {
                    pk.writeC((byte)friend.state);
                    pk.writeC(friend.removed);
                }
                Game_SyncNet.SendPacket(pk.mstream.ToArray(), gs.Connection);
            }
        }
    }
}