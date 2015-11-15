using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Wander.Server.Services
{
    public class SqlConnectionService
    {
        private static string ConnectionString;

        private static string DefaultConnection =
            @"Data Source=DESKTOP-H8QV3JS\SQLEXPRESS;Initial Catalog=WanderDB;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        /// <summary>
        /// Create a new SqlConnection
        /// </summary>
        /// <returns>Returns the new SqlConnection</returns>
        public static SqlConnection GetConnection()
        {
            if (ConnectionString == null)
            {
                try
                {
                    ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                    ConnectionString = DefaultConnection;
                }
            }
            return new SqlConnection(ConnectionString);
        }
    }
}