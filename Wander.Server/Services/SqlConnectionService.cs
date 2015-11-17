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
            @"Data Source=(localdb)\ProjectsV12;Initial Catalog=WanderDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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
            }
            else
            {
                ConnectionString = db;
            }
            
            return new SqlConnection(ConnectionString);
        }
    }
}