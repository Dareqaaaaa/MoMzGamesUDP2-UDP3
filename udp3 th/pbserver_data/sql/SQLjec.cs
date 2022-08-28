/*
 * Arquivo: SQLjec.cs
 * Código criado pela MoMz Games
 * Última data de modificação: 16/09/2017
 * Sintam inveja, não nos atinge
 */

using Npgsql;
using System.Runtime.Remoting.Contexts;

namespace Core.sql
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
                Database = ConfigGB.dbName,
                Host = ConfigGB.dbHost,
                Username = ConfigGB.dbUser,
                Password = ConfigGB.dbPass,
                Port = ConfigGB.dbPort
            };
            //connBuilder.ConnectionIdleLifetime = 0; //??
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