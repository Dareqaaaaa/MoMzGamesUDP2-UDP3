/*
 * Arquivo: AccountManager.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 03/07/2017
 * Sintam inveja, não nos atinge
 */

using Core;
using Core.managers;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Game.data.managers
{
    public static class ClassicModeManager
    {
        public static List<int> itemscamp = new List<int>();
        public static List<int> itemscnpb = new List<int>();
        public static List<int> items79 = new List<int>();
        public static List<int> itemslan = new List<int>();
        public static void LoadList()
        {
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = "SELECT * FROM tournament_rules";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        string tournament1 = data.GetString(0);
                        string filter = data.GetString(1);
                        if (tournament1 == "camp")
                        { ShopManager.IsBlocked(filter, itemscamp); }
                        if (tournament1 == "cnpb")
                        { ShopManager.IsBlocked(filter, itemscnpb); }
                        if (tournament1 == "79")
                        { ShopManager.IsBlocked(filter, items79); }
                        if (tournament1 == "lan")
                        { ShopManager.IsBlocked(filter, itemslan); }

                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }            
                Logger.warning("Trounament Rules @Camp Count: " + itemscamp.Count);
                Logger.warning("Trounament Rules @Cnpb Count: " + itemscnpb.Count);
                Logger.warning("Trounament Rules @79 Count: " + items79.Count);
                Logger.warning("Trounament Rules @lan Count: " + itemslan.Count);
            }
            catch (Exception ex)
            {
                Logger.error("Ocorreu um problema ao carregar os Tournament Rules!\r\n" + ex.ToString());
            }
        }
        public static bool IsBlocked(int listid, int id)
        {
            if (listid == id)
                return true;
            return false;
        }
        public static bool IsBlocked(int listid, int id, ref List<string> list, string category)
        {
            if (listid == id)
            {
                list.Add(category);
                return true;
            }
            return false;
        }
    }
}