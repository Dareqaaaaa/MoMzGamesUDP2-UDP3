using Auth.data.managers;
using Auth.data.model;
using Core.models.account;
using Core.server;

namespace Auth.data.sync.client_side
{
    public static class Net_Friend_Sync
    {
        public static void Load(ReceiveGPacket p)
        {
            long playerId = p.readQ();
            int type = p.readC();

            long friendId = p.readQ();
            int state;
            bool removed;
            Friend friendModel = null;
            if (type <= 1)
            {
                state = p.readC();
                removed = p.readC() == 1;
                friendModel = new Friend(friendId) { state = state, removed = removed };
            }
            if (friendModel == null && type <= 1)
                return;

            Account player = AccountManager.getInstance().getAccount(playerId, true);
            if (player != null)
            {
                if (type <= 1)
                {
                    friendModel.player.player_name = player.player_name;
                    friendModel.player._rank = player._rank;
                    friendModel.player._isOnline = player._isOnline;
                    friendModel.player._status = player._status;
                }

                if (type == 0) //Adicionar
                    player.FriendSystem.AddFriend(friendModel);
                else if (type == 1) //Atualizar
                {
                    Friend myFriend = player.FriendSystem.GetFriend(friendId);
                    if (myFriend != null)
                        myFriend = friendModel;
                }
                else if (type == 2) //Deletar
                    player.FriendSystem.RemoveFriend(friendId);
            }
        }
    }
}