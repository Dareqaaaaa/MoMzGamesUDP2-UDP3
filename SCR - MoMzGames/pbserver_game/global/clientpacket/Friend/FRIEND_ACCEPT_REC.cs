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
    public class FRIEND_ACCEPT_REC : ReceiveGamePacket
    {
        private int index;
        private uint erro;
        public FRIEND_ACCEPT_REC(GameClient client, byte[] data)
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
                Friend friend = p.FriendSystem.GetFriend(index);
                if (friend != null && friend.state > 0)
                {
                    Account _f = AccountManager.getAccount(friend.player_id, 32);
                    if (_f != null)
                    {
                        if (friend.player == null)
                            friend.SetModel(_f.player_id, _f._rank, _f.player_name, _f._isOnline, _f._status);
                        else
                            friend.player.SetInfo(_f._rank, _f.player_name, _f._isOnline, _f._status);

                        friend.state = 0;
                        PlayerManager.UpdateFriendState(p.player_id, friend);
                        _client.SendPacket(new FRIEND_UPDATE_PAK(FriendChangeState.Accept, null, 0, index));
                        _client.SendPacket(new FRIEND_UPDATE_PAK(FriendChangeState.Update, friend, index));
                        int idx = -1;
                        Friend fs = _f.FriendSystem.GetFriend(p.player_id, out idx);
                        if (fs != null && fs.state > 0)
                        {
                            if (fs.player == null)
                                fs.SetModel(p.player_id, p._rank, p.player_name, p._isOnline, p._status);
                            else
                                fs.player.SetInfo(p._rank, p.player_name, p._isOnline, p._status);

                            fs.state = 0;
                            PlayerManager.UpdateFriendState(_f.player_id, fs);
                            SEND_FRIENDS_INFOS.Load(_f, fs, 1);
                            _f.SendPacket(new FRIEND_UPDATE_PAK(FriendChangeState.Update, fs, idx), false);
                        }
                    }
                    else erro = 0x80000000;//STR_TBL_GUI_BASE_NO_USER_IN_USERLIST
                }
                else erro = 0x80000000;//STR_TBL_GUI_BASE_NO_USER_IN_USERLIST
                if (erro > 0)
                    _client.SendPacket(new FRIEND_ACCEPT_PAK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("[FRIEND_ACCEPT_REC] " + ex.ToString());
            }
        }
    }
}