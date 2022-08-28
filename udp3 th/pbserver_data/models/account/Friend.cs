/*
 * Arquivo: Friend.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 28/04/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account.players;

namespace Core.models.account
{
    public class Friend
    {
        public long player_id;
        public int state;
        public bool removed;
        public PlayerInfo player;
        public Friend(long player_id)
        {
            this.player_id = player_id;
            player = new PlayerInfo(player_id);
        }
        public Friend(long player_id, int rank, string name, bool isOnline, AccountStatus status)
        {
            this.player_id = player_id;
            SetModel(player_id, rank, name, isOnline, status);
        }
        public void SetModel(long player_id, int rank, string name, bool isOnline, AccountStatus status)
        {
            player = new PlayerInfo(player_id, rank, name, isOnline, status);
        }
    }
}