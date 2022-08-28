/*
 * Arquivo: SQLjec.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 05/01/2017
 * Sinta inveja, não nos atinge
 */

using Battle.config;
using Npgsql;
using System.Runtime.Remoting.Contexts;

namespace Battle
{
    [Synchronization]
    public class SQLjec
    {
        private static SQLjec sql = new SQLjec();
        protected NpgsqlConnectionStringBuilder connBuilder;

        static SQLjec()
        {
        }

        public SQLjec()
        {
            connBuilder = new NpgsqlConnectionStringBuilder
            {
                Database = Config.dbName,
                Host = Config.dbHost,
                Username = Config.dbUser,
                Password = Config.dbPass,
                Port = Config.dbPort
            };
        }

        public static SQLjec getInstance()
        {
            return sql;
        }

        public NpgsqlConnection conn()
        {
            return new NpgsqlConnection(connBuilder.ConnectionString);
        }
    }
}