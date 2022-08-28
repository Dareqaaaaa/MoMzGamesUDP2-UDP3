/*
 * Arquivo: LoggerGS.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/06/2017
 * Sintam inveja, não nos atinge
 */

using Game.data.managers;
using System;
using System.Threading.Tasks;

namespace Game
{
    public static class LoggerGS
    {
        public static int TestSlot = 1;
        public static async void updateRAM()
        {
            while (true)
            {
                Console.Title = "Point Blank - Game [Server: " + ConfigGS.serverId + "; Users: " + GameManager._socketList.Count + "; Loaded accs: " + AccountManager._contas.Count + "; Used RAM: " + (GC.GetTotalMemory(true) / 1024) + " KB]";
                await Task.Delay(1000);
            }
        }
    }
}