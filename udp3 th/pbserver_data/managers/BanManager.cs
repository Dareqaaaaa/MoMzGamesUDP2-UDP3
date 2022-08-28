﻿using Core.server;
using Core.sql;
using Npgsql;
using System;

namespace Core.managers
{
    public static class BanManager
    {
        public static BanHistory GetAccountBan(long object_id)
        {
            BanHistory ban = new BanHistory();
            if (object_id == 0)
                return ban;
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@obj", object_id);
                    command.CommandText = "SELECT * FROM ban_history WHERE object_id=@obj";
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        ban.object_id = object_id;
                        ban.provider_id = data.GetInt64(1);
                        ban.type = data.GetString(2);
                        ban.value = data.GetString(3);
                        ban.reason = data.GetString(4);
                        ban.startDate = data.GetDateTime(5);
                        ban.endDate = data.GetDateTime(6);
                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
                return null;
            }
            return ban;
        }
        public static void GetBanStatus(string mac, string ip, out bool validMac, out bool validIp)
        {
            validMac = false;
            validIp = false;
            try
            {
                DateTime now = DateTime.Now;
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@mac", mac);
                    command.Parameters.AddWithValue("@ip", ip);
                    command.CommandText = "SELECT * FROM ban_history WHERE value in (@mac,@ip)";
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        string type = data.GetString(2);
                        string value = data.GetString(3);
                        DateTime endDate = data.GetDateTime(6);
                        if (endDate < now)
                            continue;
                        if (type == "MAC" && value == mac)
                            validMac = true;
                        else if (type == "IP" && value == ip)
                            validIp = true;
                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.warning(ex.ToString());
            }
        }
        public static BanHistory SaveHistory(long provider, string type, string value, DateTime end)
        {
            BanHistory ban = new BanHistory()
            {
                provider_id = provider,
                type = type,
                value = value,
                endDate = end
            };
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@provider", ban.provider_id);
                    command.Parameters.AddWithValue("@type", ban.type);
                    command.Parameters.AddWithValue("@value", ban.value);
                    command.Parameters.AddWithValue("@reason", ban.reason);
                    command.Parameters.AddWithValue("@start", ban.startDate);
                    command.Parameters.AddWithValue("@end", ban.endDate);
                    command.CommandText = "INSERT INTO ban_history(provider_id,type,value,reason,start_date,expire_date)VALUES(@provider,@type,@value,@reason,@start,@end) RETURNING object_id";
                    object data = command.ExecuteScalar();
                    ban.object_id = (long)data;
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                    return ban;
                }
            }
            catch 
            { 
                return null; 
            }
        }
        public static bool SaveBanReason(long object_id, string reason)
        {
            return ComDiv.updateDB("ban_history", "reason", reason, "object_id", object_id);
        }
    }
    public class BanHistory
    {
        public long object_id, provider_id;
        public string type, value, reason;
        public DateTime startDate, endDate;
        public BanHistory()
        {
            startDate = DateTime.Now;
            type = "";
            value = "";
            reason = "";
        }
    }
}