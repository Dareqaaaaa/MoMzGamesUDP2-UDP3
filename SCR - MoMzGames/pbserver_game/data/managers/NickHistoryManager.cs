/*
 * Arquivo: NickHistoryManager.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 19/03/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Game.data.managers
{
    public static class NickHistoryManager
    {
        public static List<NHistoryModel> getHistory(object valor, int type)
        {
            List<NHistoryModel> nicks = new List<NHistoryModel>();
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    string moreCmd = type == 0 ? "WHERE to_nick=@valor" : "WHERE player_id=@valor";
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@valor", valor);
                    command.CommandText = "SELECT * FROM nick_history " + moreCmd + " ORDER BY change_date LIMIT 30";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        nicks.Add(new NHistoryModel
                        {
                            player_id = data.GetInt64(0),
                            from_nick = data.GetString(1),
                            to_nick = data.GetString(2),
                            date = (UInt32)data.GetInt64(3),
                            motive = data.GetString(4)
                        });
                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch
            {
                Logger.error("Ocorreu um problema ao carregar o histórico de apelidos!");
            }
            return nicks;
        }
        public static bool CreateHistory(long player_id, string old_nick, string new_nick, string motive)
        {
            NHistoryModel history = new NHistoryModel
            {
                player_id = player_id,
                from_nick = old_nick,
                to_nick = new_nick,
                date = uint.Parse(DateTime.Now.ToString("yyMMddHHmm")),
                motive = motive
            };
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@owner", history.player_id);
                    command.Parameters.AddWithValue("@oldnick", history.from_nick);
                    command.Parameters.AddWithValue("@newnick", history.to_nick);
                    command.Parameters.AddWithValue("@date", (long)history.date);
                    command.Parameters.AddWithValue("@motive", history.motive);
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO nick_history(player_id,from_nick,to_nick,change_date,motive)VALUES(@owner,@oldnick,@newnick,@date,@motive)";
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();
                    return true;
                }
            }
            catch { return false; }
        }
    }
    public class NHistoryModel
    {
        public string from_nick, to_nick, motive;
        public long player_id;
        public uint date;
    }
}