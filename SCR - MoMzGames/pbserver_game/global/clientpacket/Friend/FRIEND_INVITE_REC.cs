using Core;
using Core.models.account;
using Core.models.enums.friends;
using Game.data.managers;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class FRIEND_INVITE_REC : ReceiveGamePacket
    {
        private string playerName;
        private int idx1, idx2;
        public FRIEND_INVITE_REC(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            playerName = readS(33);
        }

        public override void run()
        {
            try
            {
                if (_client == null)
                    return;
                Account player = _client._player;
                if (player == null || player.player_name.Length == 0 || player.player_name == playerName)
                    _client.SendPacket(new FRIEND_INVITE_PAK(0x80001037));
                else if (player.FriendSystem._friends.Count >= 50)
                    _client.SendPacket(new FRIEND_INVITE_PAK(0x80001038));
                else
                {
                    Account friend = AccountManager.getAccount(playerName, 1, 32);
                    if (friend != null)
                    {
                        if (player.FriendSystem.GetFriendIdx(friend.player_id) == -1)
                        {
                            if (friend.FriendSystem._friends.Count >= 50)
                                _client.SendPacket(new FRIEND_INVITE_PAK(0x80001038));
                            else
                            {
                                int erro = AllUtils.AddFriend(friend, player, 2);
                                if (AllUtils.AddFriend(player, friend, (erro == 1 ? 0 : 1)) == -1 || erro == -1)
                                {
                                    _client.SendPacket(new FRIEND_INVITE_PAK(0x80001039));
                                    return;
                                }
                                Friend f1 = player.FriendSystem.GetFriend(friend.player_id, out idx1);
                                Friend f2 = friend.FriendSystem.GetFriend(player.player_id, out idx2);
                                if (f2 != null)
                                    friend.SendPacket(new FRIEND_UPDATE_PAK(erro == 0 ? FriendChangeState.Insert : FriendChangeState.Update, f2, idx2), false);
                                if (f1 != null)
                                    _client.SendPacket(new FRIEND_UPDATE_PAK(FriendChangeState.Insert, f1, idx1));
                            }
                        }
                        else _client.SendPacket(new FRIEND_INVITE_PAK(0x80001041));
                    }
                    else _client.SendPacket(new FRIEND_INVITE_PAK(0x80001042));
                }
            }
            catch (Exception ex)
            {
                Logger.info("[FRIEND_INVITE_REC] " + ex.ToString());
            }
        }
    }
}