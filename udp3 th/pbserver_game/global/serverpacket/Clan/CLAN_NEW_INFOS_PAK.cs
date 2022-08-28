/*
 * Arquivo: CLAN_NEW_INFOS_PAK.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 26/07/2017
 * Sintam inveja, não nos atinge
 */

using Core.models.account.clan;
using Core.server;
using Game.data.managers;
using Game.data.model;

namespace Game.global.serverpacket
{
    public class CLAN_NEW_INFOS_PAK : SendPacket
    {
        private Clan clan;
        private Account p;
        private int players;
        public CLAN_NEW_INFOS_PAK(Clan c, Account owner, int clanPlayers)
        {
            clan = c;
            p = owner;
            players = clanPlayers;
        }
        public CLAN_NEW_INFOS_PAK(Clan c, int clanPlayers)
        {
            clan = c;
            p = AccountManager.getAccount(clan.owner_id, 0);
            players = clanPlayers;
        }
        public override void write()
        {
            writeH(1328);
            writeD(clan._id);
            writeS(clan._name, 17);
            writeC((byte)clan._rank);
            writeC((byte)players);
            writeC((byte)clan.maxPlayers);
            writeD(clan.creationDate);
            writeD(clan._logo);
            writeC((byte)clan._name_color);
            writeC((byte)clan.getClanUnit(players));
            writeD(clan._exp);
            writeD(0);
            writeQ(clan.owner_id);
            writeS(p != null ? p.player_name : "", 33);
            writeC((byte)(p != null ? p._rank : 0));
            writeS(clan._info, 255);
            writeS("Temp", 21);
            writeC((byte)clan.limite_rank);
            writeC((byte)clan.limite_idade);
            writeC((byte)clan.limite_idade2);
            writeC((byte)clan.autoridade);
            writeS(clan._news, 255);
            writeD(clan.partidas);
            writeD(clan.vitorias);
            writeD(clan.derrotas);
            writeD(clan.partidas);
            writeD(clan.vitorias);
            writeD(clan.derrotas);
            //MELHORES MEMBROS DO CLÃ
            writeQ(clan.BestPlayers.Exp.PlayerId); //XP Adquirida (Total)
            writeQ(clan.BestPlayers.Exp.PlayerId); //XP Adquirida (Temporada)
            writeQ(clan.BestPlayers.Wins.PlayerId); //Vitória (Total)
            writeQ(clan.BestPlayers.Wins.PlayerId); //Vitória (Temporada)
            writeQ(clan.BestPlayers.Kills.PlayerId); //Kills (Total)
            writeQ(clan.BestPlayers.Kills.PlayerId); //Kills (Temporada)
            writeQ(clan.BestPlayers.Headshot.PlayerId); //Headshots (Total)
            writeQ(clan.BestPlayers.Headshot.PlayerId); //Headshots (Temporada)
            writeQ(clan.BestPlayers.Participation.PlayerId); //Participação (Total)
            writeQ(clan.BestPlayers.Participation.PlayerId); //Participação (Temporada)
            writeT(clan._pontos);
        }
    }
}