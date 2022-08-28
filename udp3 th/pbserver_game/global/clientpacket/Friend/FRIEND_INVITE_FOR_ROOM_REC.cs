using Core;
using Core.models.account;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
    public class FRIEND_INVITE_FOR_ROOM_REC : ReceiveGamePacket
    {
        private int index;
        public FRIEND_INVITE_FOR_ROOM_REC(GameClient client, byte[] data)
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
                Account fr = GetFriend(p);
                if (fr != null)
                {
                    if (fr._status.serverId == 255 || fr._status.serverId == 0)
                    {
                        _client.SendPacket(new FRIEND_INVITE_FOR_ROOM_PAK(0x80003002));
                        return;
                    }
                    else if (fr.matchSlot >= 0)
                    {
                        _client.SendPacket(new FRIEND_INVITE_FOR_ROOM_PAK(0x80003003));
                        return;
                    }
                    int pIdx = fr.FriendSystem.GetFriendIdx(p.player_id);
                    if (pIdx == -1)
                        _client.SendPacket(new FRIEND_INVITE_FOR_ROOM_PAK(0x8000103E));
                    else if (fr._isOnline)
                        fr.SendPacket(new FRIEND_ROOM_INVITE_PAK(pIdx), false);
                    else
                        _client.SendPacket(new FRIEND_INVITE_FOR_ROOM_PAK(0x8000103F));
                }
                else
                    _client.SendPacket(new FRIEND_INVITE_FOR_ROOM_PAK(0x8000103D));
            }
            catch (Exception ex)
            {
                Logger.info("[FRIEND_INVITE_FOR_ROOM_REC] " + ex.ToString());
            }
        }
        private Account GetFriend(Account p)
        {
            Friend friend = p.FriendSystem.GetFriend(index);
            if (friend == null)
                return null;
            return AccountManager.getAccount(friend.player_id, 32);
        }
    }
}