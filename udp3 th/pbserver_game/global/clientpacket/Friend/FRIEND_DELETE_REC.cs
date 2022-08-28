using Core;
using Core.managers;
using Core.models.account;
using Core.models.enums.friends;
using Game.data.managers;
using Game.data.model;
using Game.data.sync.server_side;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class FRIEND_DELETE_REC : ReceiveGamePacket
    {
        private int index;
        private uint erro;
        public FRIEND_DELETE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            index = readC();
        }

        public override void run()
        {
            try
            {
                Account p = _client._player; 
                if (p == null)
                    return;
                Friend f = p.FriendSystem.GetFriend(index);
                if (f != null)
                {
                    PlayerManager.DeleteFriend(f.player_id, p.player_id);
                    Account friend = AccountManager.getAccount(f.player_id, 32);
                    if (friend != null)
                    {
                        int idx = -1;
                        Friend f2 = friend.FriendSystem.GetFriend(p.player_id, out idx);
                        if (f2 != null)
                        {
                            f2.removed = true;
                            PlayerManager.UpdateFriendBlock(friend.player_id, f2);
                            SEND_FRIENDS_INFOS.Load(friend, f2, 2);
                            friend.SendPacket(new FRIEND_UPDATE_PAK(FriendChangeState.Update, f2, idx), false);
                        }
                    }
                    p.FriendSystem.RemoveFriend(f);
                    _client.SendPacket(new FRIEND_UPDATE_PAK(FriendChangeState.Delete, null, 0, index));
                }
                else
                    erro = 0x80000000;
                _client.SendPacket(new FRIEND_REMOVE_PAK(erro));
                _client.SendPacket(new FRIEND_MY_FRIENDLIST_PAK(p.FriendSystem._friends));
            }
            catch (Exception ex)
            {
                Logger.info("[FRIEND_DELETE_REC] " + ex.ToString());
            }
        }
    }
}