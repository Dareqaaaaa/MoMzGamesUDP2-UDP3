using Core;
using Core.models.enums;
using System.Text;

namespace Game
{
    public static class ConfigGS
    {
        public static string passw, gameIp;
        public static bool isTestMode, debugMode, winCashPerBattle, showCashReceiveWarn, EnableClassicRules;
        public static SERVER_UDP_STATE udpType;
        public static float maxClanPoints;
        public static int serverId, configId, maxBattleLatency, maxRepeatLatency, syncPort, maxActiveClans, minRankVote, maxNickSize, minNickSize, maxBattleXP, maxBattleGP, maxBattleMY, maxChannelPlayers, gamePort, minCreateGold, minCreateRank;
        public static void Load()
        {
            ConfigFile configFile = new ConfigFile("config/game.ini");
            ConfigGB.dbHost = configFile.readString("dbhost", "localhost");
            ConfigGB.dbName = configFile.readString("dbname", "");
            ConfigGB.dbUser = configFile.readString("dbuser", "root");
            ConfigGB.dbPass = configFile.readString("dbpass", "");
            ConfigGB.dbPort = configFile.readInt32("dbport", 0);

            serverId = configFile.readInt32("serverId", -1);
            configId = configFile.readInt32("configId", 0);
            gameIp = configFile.readString("gameIp","127.0.0.1");
            gamePort = configFile.readInt32("gamePort", 39190);
            syncPort = configFile.readInt32("syncPort", 0);
            debugMode = configFile.readBoolean("debugMode", true);
            isTestMode = configFile.readBoolean("isTestMode", true);
            ConfigGB.EncodeText = Encoding.GetEncoding(configFile.readInt32("EncodingPage", 0));
            EnableClassicRules = configFile.readBoolean("EnableClassicRules", false);
            winCashPerBattle = configFile.readBoolean("winCashPerBattle", true);
            showCashReceiveWarn = configFile.readBoolean("showCashReceiveWarn", true);
            minCreateRank = configFile.readInt32("minCreateRank", 15);
            minCreateGold = configFile.readInt32("minCreateGold", 7500);
            maxClanPoints = configFile.readFloat("maxClanPoints", 0);
            passw = configFile.readString("passw", "");
            maxChannelPlayers = configFile.readInt32("maxChannelPlayers", 100);
            maxBattleXP = configFile.readInt32("maxBattleXP", 1000);
            maxBattleGP = configFile.readInt32("maxBattleGP", 1000);
            maxBattleMY = configFile.readInt32("maxBattleMY", 1000);
            udpType = (SERVER_UDP_STATE)configFile.readByte("udpType", 1);
            minNickSize = configFile.readInt32("minNickSize", 0);
            maxNickSize = configFile.readInt32("maxNickSize", 0);
            minRankVote = configFile.readInt32("minRankVote", 0);
            maxActiveClans = configFile.readInt32("maxActiveClans", 0);
            maxBattleLatency = configFile.readInt32("maxBattleLatency", 0);
            maxRepeatLatency = configFile.readInt32("maxRepeatLatency", 0);
        }
    }
}