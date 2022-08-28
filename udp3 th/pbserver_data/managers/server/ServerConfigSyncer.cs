/*
 * Arquivo: ServerConfigSyncer.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 18/10/2017
 * Sintam inveja, não nos atinge
 */

using Core.server;
using Core.sql;
using Npgsql;
using System;
using System.Data;

namespace Core.managers.server
{
    public static class ServerConfigSyncer
    {
        public static ServerConfig GenerateConfig(int configId)
        {
            ServerConfig cfg = null;
            if (configId == 0)
                return cfg;
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@cfg", configId);
                    command.CommandText = "SELECT * FROM info_login_configs WHERE config_id=@cfg";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        cfg = new ServerConfig
                        {
                            configId = configId,
                            onlyGM = data.GetBoolean(1),
                            missions = data.GetBoolean(2),
                            UserFileList = data.GetString(3),
                            ClientVersion = data.GetString(4),
                            GiftSystem = data.GetBoolean(5),
                            ExitURL = data.GetString(6)
                        };
                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
            return cfg;
        }
        public static bool updateMission(ServerConfig cfg, bool mission)
        {
            cfg.missions = mission;
            return ComDiv.updateDB("info_login_configs", "missions", mission, "config_id", cfg.configId);
        }
    }
    public class ServerConfig
    {
        public int configId;
        public string UserFileList, ClientVersion, ExitURL;
        public bool onlyGM, missions, GiftSystem;
    }
}