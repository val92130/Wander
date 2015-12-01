using System;
using System.Data.SqlClient;

namespace Wander.Server.ClassLibrary.Services
{
    public class SqlConnectionService
    {
        private static string ConnectionString;

        private static string DefaultConnection =
        @"Data Source=(localdb)\ProjectsV12;Initial Catalog = WanderDB; Integrated Security = True;";
        /// <summary>
        /// Create a new SqlConnection
        /// </summary>
        /// <returns>Returns the new SqlConnection</returns>
        public static SqlConnection GetConnection()
        {
            string db = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (db == null)
            {
                if (ConnectionString == null)
                {
                    ConnectionString = DefaultConnection;
                }
            }
            else
            {
                ConnectionString = db;
            }

            return new SqlConnection(ConnectionString);
        }
    }
}
