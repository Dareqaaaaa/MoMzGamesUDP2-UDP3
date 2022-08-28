/*
 * Arquivo: EventXmasSyncer.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 26/06/2017
 * Sintam inveja, não nos atinge
 */

using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.managers.events
{
    public class EventXmasSyncer
    {
        private static List<EventXmasModel> _events = new List<EventXmasModel>();
        public static void GenerateList()
        {
            try
            {
                using (NpgsqlConnection connection = SQLjec.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = "SELECT * FROM events_xmas";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        EventXmasModel ev = new EventXmasModel
                        {
                            startDate = (UInt32)data.GetInt64(0),
                            endDate = (UInt32)data.GetInt64(1)
                        };
                        _events.Add(ev);
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
        public static void ReGenList()
        {
            _events.Clear();
            GenerateList();
        }
        public static EventXmasModel getRunningEvent()
        {
            try
            {
                uint date = uint.Parse(DateTime.Now.ToString("yyMMddHHmm"));
                for (int i = 0; i < _events.Count; i++)
                {
                    EventXmasModel ev = _events[i];
                    if (ev.startDate <= date && date < ev.endDate)
                        return ev;
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
            return null;
        }
    }
    public class EventXmasModel
    {
        public uint startDate, endDate;
    }
}