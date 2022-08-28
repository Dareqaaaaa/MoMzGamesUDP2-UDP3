using Core;
using Core.models.enums.global;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth
{
    public static class ConfigGA
    {
        public static string authIp;
        public static bool isTestMode, Outpost, AUTO_ACCOUNTS, debugMode;
        public static int syncPort, configId, maxNickSize, minNickSize, minLoginSize, minPassSize, authPort;
        public static ulong LauncherKey;
        public static List<ClientLocale> GameLocales;
        public static void Load()
        {
            ConfigFile configFile = new ConfigFile("config/auth.ini");
            ConfigGB.dbHost = configFile.readString("dbhost", "localhost");
            ConfigGB.dbName = configFile.readString("dbname", "");
            ConfigGB.dbUser = configFile.readString("dbuser", "root");
            ConfigGB.dbPass = configFile.readString("dbpass", "");
            ConfigGB.dbPort = configFile.readInt32("dbport", 0);

            configId = configFile.readInt32("configId", 0);
            authIp = configFile.readString("authIp", "127.0.0.1");
            authPort = configFile.readInt32("authPort", 39190);
            syncPort = configFile.readInt32("syncPort", 0);
            AUTO_ACCOUNTS = configFile.readBoolean("autoaccounts", false);
            debugMode = configFile.readBoolean("debugMode", true);
            isTestMode = configFile.readBoolean("isTestMode", true);
            ConfigGB.EncodeText = Encoding.GetEncoding(configFile.readInt32("EncodingPage", 0));
            Outpost = configFile.readBoolean("Outpost", false);
            LauncherKey = configFile.readUInt64("LauncherKey", 0);
            minNickSize = configFile.readInt32("minNickSize", 0);
            maxNickSize = configFile.readInt32("maxNickSize", 0);
            minLoginSize = configFile.readInt32("minLoginSize", 0);
            minPassSize = configFile.readInt32("minPassSize", 0);
            GameLocales = new List<ClientLocale>();
            string strLocales = configFile.readString("GameLocales", "None");
            foreach (string splitedLocale in strLocales.Split(','))
            {
                ClientLocale clientLocale;
                Enum.TryParse<ClientLocale>(splitedLocale, out clientLocale);
                GameLocales.Add(clientLocale);
            }
        }
    }
}