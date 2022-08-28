/*
 * Arquivo: Clan.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 09/11/2017
 * Sintam inveja, não nos atinge
 */

using Core.managers;

namespace Core.models.account.clan
{
    public class Clan
    {
        public int _id, creationDate, partidas, vitorias, derrotas, autoridade, limite_rank, limite_idade, limite_idade2, _exp, _rank, _name_color, maxPlayers = 50;
        public string _name = "", _info = "", _news = "";
        public long owner_id;
        public uint _logo = 4294967295;
        public float _pontos = 1000;
        public ClanBestPlayers BestPlayers = new ClanBestPlayers();
        /// <summary>
        /// Gera o tipo de unidade do clã através da quantia de jogadores adquiridas pela Database.
        /// </summary>
        /// <returns></returns>
        public int getClanUnit()
        {
            return getClanUnit(PlayerManager.getClanPlayers(_id));
        }
        /// <summary>
        /// Gerar o tipo de unidade do clã através da quantia de jogadores fornecida pelo aplicativo.
        /// </summary>
        /// <param name="count">Quantia de jogadores no clã.</param>
        /// <returns></returns>
        public int getClanUnit(int count)
        {
            //Possível 8 - "Top"
            if (count >= 250) return 7; //Corpo
            else if (count >= 200) return 6; //Divisão
            else if (count >= 150) return 5; //Brigada
            else if (count >= 100) return 4; //Regimento
            else if (count >= 50) return 3; //Batalhão
            else if (count >= 30) return 2; //Companhia
            else if (count >= 10) return 1; //Pelotão
            else return 0; //Esquadra
        }
    }
}