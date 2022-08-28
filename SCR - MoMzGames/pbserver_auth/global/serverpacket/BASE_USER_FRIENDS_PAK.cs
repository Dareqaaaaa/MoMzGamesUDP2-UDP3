/*
 * Arquivo: BASE_USER_FRIENDS_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 21/04/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account;
using Core.models.account.players;
using Core.server;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
    public class BASE_USER_FRIENDS_PAK : SendPacket
    {
        private List<Friend> friends;
        public BASE_USER_FRIENDS_PAK(List<Friend> friends)
        {
            this.friends = friends;
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