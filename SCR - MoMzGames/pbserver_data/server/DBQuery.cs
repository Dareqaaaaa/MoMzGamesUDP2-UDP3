/*
 * Arquivo: ShopManager.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 08/08/2017
 * Sintam inveja, não nos atinge
 */
using System.Collections.Generic;

namespace Core.server
{
    public class DBQuery
    {
        private List<string> tables;
        private List<object> values;
        public DBQuery()
        {
            tables = new List<string>();
            values = new List<object>();
        }

        public void AddQuery(string table, object value)
        {
            tables.Add(table);
            values.Add(value);
        }

        public string[] GetTables()
        {
            return tables.ToArray();
        }
        public object[] GetValues()
        {
            return values.ToArray();
        }
    }
}