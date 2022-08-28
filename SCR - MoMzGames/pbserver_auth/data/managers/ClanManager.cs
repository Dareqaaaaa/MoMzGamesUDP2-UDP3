/*
 * Arquivo: ClanManager.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 10/07/2017
 * Sintam inveja, não nos atinge
 */

using Auth.data.model;
using Core;
using Core.models.account.clan;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Auth.data.managers
{
    public class ClanManager
    {
        /// <summary>
        /// Procura Clã na Database, seguindo alguns parâmetros.
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="type">Tipo de procura. 0 = Nome; 1 = Id</param>
        /// <returns></returns>
        public static Clan getClanDB(object valor, int type)
        {
            try
            {
                Clan c = new Clan();
                if (type == 1 && (int)valor <= 0 || type == 0 && string.IsNullOrEmpty(valor.ToString()))
                    return c;
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    string moreCmd = type == 0 ? "clan_name" : "clan_id";
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@valor", valor);
                    command.CommandText = "SELECT * FROM clan_data WHERE " + moreCmd + "=@valor";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        c._id = data.GetInt32(0);
                        c._rank = data.GetInt32(1);
                        c._name = data.GetString(2);
                        c.owner_id = data.GetInt64(3);
                        c._logo = (UInt32)data.GetInt64(4);
                        c._name_color = data.GetInt32(5);
                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }
                return c._id == 0 ? new Clan() : c;
            }
            catch
            {
                return new Clan();
            }
        }
        public static List<Account> getClanPlayers(int clanId, long exception)
        {
            List<Account> friends = new List<Account>();
            if (clanId <= 0)
                return friends;
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@clan", clanId);
                    command.CommandText = "SELECT player_id,player_name,rank,online,status FROM accounts WHERE clan_id=@clan";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        long pId = data.GetInt64(0);
                        if (pId == exception)
                            continue;

                        Account p = new Account
                        {
                            player_id = pId,
                            player_name = data.GetString(1),
                            _rank = data.GetInt32(2),
                            _isOnline = data.GetBoolean(3)
                        };
                        p._status.SetData((uint)data.GetInt64(4), pId);
                        if (p._isOnline && !AccountManager.getInstance()._contas.ContainsKey(pId))
                        {
                            p.setOnlineStatus(false);
                            p._status.ResetData(p.player_id);
                        }
                        friends.Add(p);
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
            return friends;
        }
        public static List<Account> getClanPlayers(int clanId, long exception, bool isOnline)
        {
            List<Account> friends = new List<Account>();
            if (clanId <= 0)
                return friends;
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@clan", clanId);
                    command.Parameters.AddWithValue("@on", isOnline);
                    command.CommandText = "SELECT player_id,player_name,rank,online,status FROM accounts WHERE clan_id=@clan AND online=@on";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        long pId = data.GetInt64(0);
                        if (pId == exception)
                            continue;

                        Account p = new Account
                        {
                            player_id = pId,
                            player_name = data.GetString(1),
                            _rank = data.GetInt32(2),
                            _isOnline = data.GetBoolean(3)
                        };
                        p._status.SetData((uint)data.GetInt64(4), pId);
                        if (p._isOnline && !AccountManager.getInstance()._contas.ContainsKey(pId))
                        {
                            p.setOnlineStatus(false);
                            p._status.ResetData(p.player_id);
                        }
                        friends.Add(p);
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
            return friends;
        }
    }
}