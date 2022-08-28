/*
 * Arquivo: PlayerStats.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 02/11/2017
 * Sintam inveja, não nos atinge
 */

using System;

namespace Core.models.account.players
{
    public class PlayerStats
    {
        public int fights, fights_win, fights_lost, fights_draw, kills_count, totalkills_count, totalfights_count, deaths_count, escapes, headshots_count;
        public int ClanGames, ClanWins;
        public int GetKDRatio()
        {
            if (headshots_count <= 0 && kills_count <= 0)
                return 0;
            return (int)Math.Floor((kills_count * 100 + 0.5) / (double)(kills_count + deaths_count));
        }
        public int GetHSRatio()
        {
            if (kills_count <= 0)
                return 0;
            return (int)Math.Floor((headshots_count * 100) / (double)kills_count + 0.5);
        }
    }
}