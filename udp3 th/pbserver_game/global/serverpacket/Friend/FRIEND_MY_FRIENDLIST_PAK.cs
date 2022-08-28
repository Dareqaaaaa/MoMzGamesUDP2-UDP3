using Core.models.account;
using Core.models.account.players;
using Core.server;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class FRIEND_MY_FRIENDLIST_PAK : SendPacket
    {
        private List<Friend> friends;
        public FRIEND_MY_FRIENDLIST_PAK(List<Friend> frie)
        {
            friends = frie;
        }

        public override void write()
        {
            writeH(274);
            writeC((byte)friends.Count);
            for (int i = 0; i < friends.Count; i++)
            {
                Friend f = friends[i];
                PlayerInfo info = f.player;
                if (info == null)
                    writeB(new byte[17]);
                else
                {
                    writeC((byte)(info.player_name.Length + 1));
                    writeS(info.player_name, info.player_name.Length + 1);
                    writeQ(f.player_id);
                    writeD(ComDiv.GetFriendStatus(f));
                    writeC((byte)info._rank);
                    writeH(0);
                    writeC(0);
                }
            }
        }
    }
}