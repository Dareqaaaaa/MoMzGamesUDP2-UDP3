using Core.models.account.players;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.xml
{
    public class BasicInventoryXML
    {
        public static List<ItemsModel> basic = new List<ItemsModel>(),
            creationAwards = new List<ItemsModel>();
        public static void Load()
        {
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = "SELECT * FROM info_basic_items";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        int rewardType = data.GetInt32(0);
                        ItemsModel item = new ItemsModel(data.GetInt32(1))
                        {
                            _name = data.GetString(2),
                            _count = (uint)data.GetInt32(3),
                            _equip = data.GetInt32(4)
                        };
                        if (rewardType == 0)
                            basic.Add(item);
                        else if (rewardType == 1)
                            creationAwards.Add(item);
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
        }
    }
}