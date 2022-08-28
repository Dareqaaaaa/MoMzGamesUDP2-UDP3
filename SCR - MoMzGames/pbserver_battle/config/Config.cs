namespace Battle.config
{
    public static class Config
    {
        public static string dbName, dbHost, dbUser, dbPass, hosIp, serverIp, udpVersion;
        public static int dbPort;
        public static ushort hosPort, maxDrop, syncPort;
        public static bool isTestMode, sendInfoToServ, sendFailMsg, enableLog, useMaxAmmoInDrop, useHitMarker;
        public static float plantDuration, defuseDuration;
        public static void Load()
        {
            ConfigFile configFile = new ConfigFile("config/battle.ini");
            dbHost = configFile.readString("dbhost", "localhost");
            dbName = configFile.readString("dbname", "");
            dbUser = configFile.readString("dbuser", "root");
            dbPass = configFile.readString("dbpass", "");
            dbPort = configFile.readInt32("dbport", 0);

            hosIp = configFile.readString("udpIp", "0.0.0.0");
            serverIp = configFile.readString("serverIp", "0.0.0.0");
            hosPort = configFile.readUInt16("udpPort", 40000);
            isTestMode = configFile.readBoolean("isTestMode", true);
            sendInfoToServ = configFile.readBoolean("sendInfoToServ", true);
            sendFailMsg = configFile.readBoolean("sendFailMsg", true);
            enableLog = configFile.readBoolean("enableLog", true);
            maxDrop = configFile.readUInt16("maxDrop", 0);
            syncPort = configFile.readUInt16("syncPort", 0);
            plantDuration = configFile.readFloat("plantDuration", 1.0f);
            defuseDuration = configFile.readFloat("defuseDuration", 1.0f);
            useHitMarker = configFile.readBoolean("useHitMarker", false);
            useMaxAmmoInDrop = configFile.readBoolean("useMaxAmmoInDrop", true);
            udpVersion = configFile.readString("UDPVersion", "0.0");
        }
    }
}