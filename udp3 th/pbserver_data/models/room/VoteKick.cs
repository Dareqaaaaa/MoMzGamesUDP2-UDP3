/*
 * Arquivo: VoteKick.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 08/06/2017
 * Sintam inveja, não nos atinge
 */

using Core.server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.models.room
{
    public class VoteKick
    {
        public int creatorIdx, victimIdx, motive, kikar = 1, deixar = 1, allies, enemys;
        public List<int> _votes = new List<int>();
        public bool[] TotalArray = new bool[16];
        /// <summary>
        /// Define o 'creatorIdx' e 'victimIdx'.
        /// <para>Adiciona os 2 jogadores na lista de votos.</para>
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="victim"></param>
        public VoteKick(int creator, int victim)
        {
            creatorIdx = creator;
            victimIdx = victim;
            _votes.Add(creator);
            _votes.Add(victim);
        }
        /// <summary>
        /// Retorna a quantidade de jogadores que votaram e ainda estão na partida.
        /// </summary>
        /// <returns></returns>
        public int GetInGamePlayers()
        {
            int count = 0;
            for (int i = 0; i < 16; i++)
            {
                if (TotalArray[i])
                    count++;
            }
            return count;
        }
    }
}