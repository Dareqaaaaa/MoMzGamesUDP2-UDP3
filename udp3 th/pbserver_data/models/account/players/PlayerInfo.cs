using Core.server;

namespace Core.models.account.players
{
    public class PlayerInfo
    {
        public int _rank;
        private long player_id;
        public string player_name;
        public bool _isOnline;
        public AccountStatus _status;
        public PlayerInfo(long player_id)
        {
            this.player_id = player_id;
            _status = new AccountStatus();
        }
        public PlayerInfo(long player_id, int rank, string name, bool isOnline, AccountStatus status)
        {
            this.player_id = player_id;
            SetInfo(rank, name, isOnline, status);
        }
        public void setOnlineStatus(bool state)
        {
            if (_isOnline != state && ComDiv.updateDB("accounts", "online", state, "player_id", player_id))
                _isOnline = state;
        }
        public void SetInfo(int rank, string name, bool isOnline, AccountStatus status)
        {
            _rank = rank;
            player_name = name;
            _isOnline = isOnline;
            _status = status;
        }
    }
}