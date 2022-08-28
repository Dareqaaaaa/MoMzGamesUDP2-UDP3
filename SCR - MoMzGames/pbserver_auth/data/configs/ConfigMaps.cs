/*
 * Arquivo: ConfigMaps.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 25/11/2016
 * Sintam inveja, não nos atinge
 */

using Core;

namespace Auth.data.configs
{
    internal class ConfigMaps
    {
        public static int Chaos, Tutorial, Deathmatch, Destruction, Sabotage, Supression, Defense, Challenge, Dinosaur, Sniper, Shotgun, HeadHunter, Knuckle, CrossCounter, TheifMode;
        private static ConfigFile configFile;
        public static void Load()
        {
            try
            {
                configFile = new ConfigFile("data/maps/defaults.ini");
            }
            catch
            {
                Logger.error("Erro ao carregar as configurações dos mapas.");
                return;
            }
            Tutorial = configFile.readInt32("Tutorial", 0);
            Deathmatch = configFile.readInt32("Deathmatch", 1);
            Destruction = configFile.readInt32("Destruction", 25);
            Sabotage = configFile.readInt32("Sabotage", 35);
            Supression = configFile.readInt32("Supression", 11);
            Defense = configFile.readInt32("Defense", 39);
            Challenge = configFile.readInt32("Challenge", 1);
            Dinosaur = configFile.readInt32("Dinosaur", 40);
            Sniper = configFile.readInt32("Sniper", 1);
            Shotgun = configFile.readInt32("Shotgun", 1);
            HeadHunter = configFile.readInt32("HeadHunter", 0);
            Knuckle = configFile.readInt32("Knuckle", 0);
            CrossCounter = configFile.readInt32("Cross-Counter", 54);
            Chaos = configFile.readInt32("Chaos", 1);
            TheifMode = configFile.readInt32("TheifMode", 1);
        }
    }
}