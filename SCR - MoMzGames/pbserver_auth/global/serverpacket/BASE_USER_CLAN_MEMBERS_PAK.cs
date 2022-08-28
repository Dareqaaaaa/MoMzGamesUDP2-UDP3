/*
 * Arquivo: BASE_USER_CLAN_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 22/07/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.model;
using Core.server;
using System.Collections.Generic;

namespace Auth.global.serverpacket
{
    public class BASE_USER_CLAN_MEMBERS_PAK : SendPacket
    {
        private List<Account> _players;
        public BASE_USER_CLAN_MEMBERS_PAK(List<Account> players)
        {
            _players = players;
        }

        public override void write()
        {
            writeH(1349);
            writeC((byte)_players.Count);
            for (int i = 0; i < _players.Count; i++)
            {
                Account member = _players[i];
                writeC((byte)(member.player_name.Length + 1));
                writeS(member.player_name, member.player_name.Length + 1);
                writeQ(member.player_id);
                writeQ(ComDiv.GetClanStatus(member._status, member._isOnline));
                writeC((byte)member._rank);
            }
        }
    }
}