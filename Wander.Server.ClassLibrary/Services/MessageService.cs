using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Wander.Server.ClassLibrary.Model;

namespace Wander.Server.ClassLibrary.Services
{
    public class MessageService : IMessageService
    {
        public List<ChatMessageModel> GetAllMessages()
        {
            var msg = new List<ChatMessageModel>();
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query =
                    "SELECT u.UserId, u.Sex, m.Message, m.Time, u.UserLogin FROM MessageLogs m JOIN Users u on u.UserId = m.UserId ORDER BY m.MessageId DESC ";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        msg.Add(Helper.CreateChatMessage(reader["UserLogin"].ToString(),
                            Convert.ToInt32(reader["UserId"]), reader["Message"].ToString(),
                            Convert.ToInt32(reader["Sex"]), reader["Time"].ToString()));
                    }

                    conn.Close();

                    return msg;
                }
            }
        }

        public List<ChatMessageModel> GetMessagesLimit(int limit)
        {
            var msg = new List<ChatMessageModel>();

            if (limit < 0) limit = 0;
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query =
                    string.Format(
                        "SELECT TOP {0} u.UserId, u.Sex, m.Message, m.Time, u.UserLogin FROM MessageLogs m JOIN Users u on u.UserId = m.UserId ORDER BY m.MessageId DESC ",
                        limit);
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        msg.Add(Helper.CreateChatMessage(reader["UserLogin"].ToString(),
                            Convert.ToInt32(reader["UserId"]), reader["Message"].ToString(),
                            Convert.ToInt32(reader["Sex"]), reader["Time"].ToString()));
                    }

                    conn.Close();

                    return msg;
                }
            }
        }

        public bool LogMessage(ChatMessageModel message)
        {
            if (message == null) throw new ArgumentException("parameter message is null in function LogMessage");

            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "INSERT INTO MessageLogs (UserId, Message, Time) values (@Id, @Message, @Time)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Id", message.UserId);
                    cmd.Parameters.AddWithValue("@Message", message.Content);
                    cmd.Parameters.AddWithValue("@Time", DateTime.Now);


                    var lines = cmd.ExecuteNonQuery();
                    conn.Close();
                    return lines != 0;
                }
            }
        }
    }
}