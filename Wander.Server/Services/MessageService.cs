using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public class MessageService : IMessageService
    {
        public List<ChatMessageModel> GetMessagesLimit(int limit)
        {
            List<ChatMessageModel> msg = new List<ChatMessageModel>();

            if (limit < 0) limit = 0;
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = String.Format("SELECT TOP {0} u.UserId, u.Sex, m.Message, m.Time, u.UserLogin FROM MessageLogs m JOIN Users u on u.UserId = m.UserId ORDER BY m.MessageId DESC ", limit);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Limit", limit);


                    var reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        msg.Add(Helper.CreateChatMessage(reader["UserLogin"].ToString(), Convert.ToInt32(reader["UserId"]), reader["Message"].ToString(), Convert.ToInt32(reader["Sex"]), reader["Time"].ToString()));
                    }

                    conn.Close();

                    return msg;
                }
            }
        }

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