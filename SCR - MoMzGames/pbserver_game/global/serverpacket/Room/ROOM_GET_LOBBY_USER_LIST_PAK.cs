using Core.server;
using Game.data.model;
using System;
using System.Collections.Generic;

namespace Game.global.serverpacket
{
    public class ROOM_GET_LOBBY_USER_LIST_PAK : SendPacket
    {
        private List<Account> players;
        private List<int> playersIdxs;
        public ROOM_GET_LOBBY_USER_LIST_PAK(Channel ch)
        {
            players = ch.getWaitPlayers();
            playersIdxs = GetRandomIndexes(players.Count, players.Count >= 8 ? 8 : players.Count);
        }
        private List<int> GetRandomIndexes(int total, int count)
        {
            if (total == 0 || count == 0)
                return new List<int>();
            Random rnd = new Random();
            List<int> numeros = new List<int>();
            for (int i = 0; i < total; i++)
                numeros.Add(i);
            for (int i = 0; i < numeros.Count; i++)
            {
                int a = rnd.Next(numeros.Count);
                int temp = numeros[i];
                numeros[i] = numeros[a];
                numeros[a] = temp;
            }
            return numeros.GetRange(0, count);
        }
        public override void write()
        {
            writeH(3855);
            writeD(playersIdxs.Count);
            for (int i = 0; i < playersIdxs.Count; i++)
            {
                Account p = players[playersIdxs[i]];
                writeD(p.getSessionId());
                writeD(p.getRank());
                writeC((byte)(p.player_name.Length + 1));
                writeS(p.player_name, p.player_name.Length + 1);
            }
        }
    }
}