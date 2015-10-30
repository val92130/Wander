using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public class LogService
    {
        public bool LogMessage(ChatMessageModel message)
        {
            if (message == null) throw new ArgumentException("parameter message is null in function LogMessage");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("INSERT INTO MessageLogs (UserId, Message, Time) values (@Id, @Message, @Time)");
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Id", message.UserId);
                    cmd.Parameters.AddWithValue("@Message", message.Content);
                    cmd.Parameters.AddWithValue("@Time", DateTime.Now);


                    int lines = cmd.ExecuteNonQuery();
                    conn.Close();
                    return lines != 0;
                }
            }
        }
    }
}