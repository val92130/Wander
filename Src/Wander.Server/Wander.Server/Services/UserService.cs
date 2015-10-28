using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

using Wander.Server.Model;

namespace Wander.Server.Services
{


    public class UserService : IUserService
    {

        private string ExecuteQuery(string value, PlayerModel user)
        {

            if (user == null) throw new ArgumentException("parameter user is null");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT {0} from dbo.Users WHERE UserId = @ConnectionId", value);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@ConnectionId", user.UserId);


                    string data = (string)cmd.ExecuteScalar();

                    conn.Close();

                    return data;
                }
            }
        }

        private int ExecuteQueryInt(string value, PlayerModel user)
        {

            if (user == null) throw new ArgumentException("parameter user is null");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT {0} from dbo.Users WHERE UserId = @ConnectionId", value);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@ConnectionId", user.UserId);


                    int data = Convert.ToInt32(cmd.ExecuteScalar());

                    conn.Close();

                    return data;
                }
            }
        }

        public string GetUserLogin(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            PlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            return ExecuteQuery("UserLogin", user);
        }

        public string GetUserLogin(PlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteQuery("UserLogin", user);
        }

        public string GetUserEmail(PlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteQuery("Email", user);
        }

        public string GetUserEmail(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            PlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            return ExecuteQuery("Email", user);
        }

        public int GetUserSex(PlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Sex", user));
        }

        public int GetUserSex(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            PlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Sex", user));
        }

        public int GetUserBankAccount(PlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Account", user));
        }

        public int GetUserBankAccount(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            PlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Account", user));
        }

        public int GetUserPoints(PlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Points", user));
        }

        public int GetUserPoints(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            PlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Points", user));
        }

        public bool GetUserActivatedStatus(PlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return Convert.ToBoolean(ExecuteQueryInt("Activated", user));
        }

        public bool GetUserActivatedStatus(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            PlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return Convert.ToBoolean(this.ExecuteQueryInt("Activated", user));
        }

        public int GetUserJobId(PlayerModel user)
        {

            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("JobId", user));
        }

        public int GetUserJobId(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            PlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("JobId", user));
        }
    }
}
