﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Wander.Server.Services
{
    public class SqlConnectionService
    {
        /// <summary>
        /// Create a new SqlConnection
        /// </summary>
        /// <returns>Returns the new SqlConnection</returns>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(@"Data Source=(localdb)\Projects;Initial Catalog=WanderDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False");
        }
    }
}