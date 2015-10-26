using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Wander.Server.Models
{
    public class DatabaseConnection
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection("connection string");
        }
    }
}