/*
 * Arquivo: CLAN_WAR_MATCH_TEAM_INFO_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 05/02/2017
 * Sintam inveja, não nos atinge
 */

using Core.managers;
using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_WAR_MATCH_TEAM_INFO_PAK : SendPacket
    {
        private uint _erro;
        private Clan c;
        private Account leader;
        public CLAN_WAR_MATCH_TEAM_INFO_PAK(uint erro, Clan c)
        {
            _erro = erro;
            this.c = c;
            if (this.c != null)
            {
                leader = AccountManager.getAccount(this.c.owner_id, 0);
                if (leader == null) _erro = 0x80000000;
            }
        }
        public CLAN_WAR_MATCH_TEAM_INFO_PAK(uint erro)
        {
            _erro = erro;
        }
        public override void write()
        {
            writeH(1570);
            writeD(_erro);
            if (_erro == 0)
            {
                int players = PlayerManager.getClanPlayers(c._id);
                writeD(c._id);
                writeS(c._name, 17);
                writeC((byte)c._rank);
                writeC((byte)players);
                writeC((byte)c.maxPlayers);
                writeD(c.creationDate);
                writeD(c._logo);
                writeC((byte)c._name_color);
                writeC((byte)c.getClanUnit(players));
                writeD(c._exp);
                writeD(0);
                writeQ(c.owner_id);
                writeS(leader.player_name, 33);
                writeC((byte)leader._rank);
                writeS("", 255);
            }//727 bytes
        }
    }
}