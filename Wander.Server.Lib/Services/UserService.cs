using System;
using System.Data.SqlClient;

using Wander.Server.Model;

namespace Wander.Server.Services
{


    public class UserService : IUserService
    {

        private bool ExecuteUpdate( string field, string value, ServerPlayerModel user)
        {

            if (user == null) throw new ArgumentException("parameter user is null");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("UPDATE dbo.Users SET {0} = {1}  WHERE UserId = @ConnectionId", field, value );
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@ConnectionId", user.UserId);

                    
                    int lines = cmd.ExecuteNonQuery();
                    conn.Close();
                    return lines != 0;
                }
            }
        }

        private string ExecuteQuery(string value, ServerPlayerModel user)
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

        private int ExecuteQueryInt(string value, ServerPlayerModel user)
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
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            return ExecuteQuery("UserLogin", user);
        }

        public string GetUserLogin(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteQuery("UserLogin", user);
        }

        public string GetUserLoginById(int userId)
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT UserLogin from dbo.Users WHERE UserId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Id", userId);

                    string data = (string)cmd.ExecuteScalar();

                    conn.Close();

                    return data;
                }
            }
        }

        public string GetUserEmail(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteQuery("Email", user);
        }

        public string GetUserEmail(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            return ExecuteQuery("Email", user);
        }

        public int GetUserSex(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Sex", user));
        }

        public int GetUserSex(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Sex", user));
        }

        public int GetUserBankAccount(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Account", user));
        }

        public int GetUserBankAccount(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Account", user));
        }

        public int GetUserPoints(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Points", user));
        }

        public int GetUserPoints(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Points", user));
        }

        public bool GetUserActivatedStatus(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return Convert.ToBoolean(ExecuteQueryInt("Activated", user));
        }

        public bool GetUserActivatedStatus(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return Convert.ToBoolean(this.ExecuteQueryInt("Activated", user));
        }

        public int GetUserJobId(ServerPlayerModel user)
        {

            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("JobId", user));
        }

        public int GetUserJobId(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("JobId", user));
        }

        public ClientPlayerModel GetAllUserInfos(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT * FROM USERS WHERE UserId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Id", user.UserId);

                    ClientPlayerModel client = new ClientPlayerModel();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        client.Sex = Convert.ToInt32(reader["Sex"]);
                        client.Account = Convert.ToInt32(reader["Account"]);
                        client.Points = Convert.ToInt32(reader["Points"]);
                        client.Points = Convert.ToInt32(reader["Points"]);
                        client.UserName = reader["UserLogin"].ToString();
                        client.Email = reader["Email"].ToString();    
                        break;                    
                    }

                    conn.Close();

                    return client;
                }
            }

        }

        public bool SetUserBankAccount(ServerPlayerModel user, int ammount)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            int userBankAccount = GetUserBankAccount(user);
            if ((userBankAccount - ammount) < 0)
            {
                ammount = 0;
            }
            return ExecuteUpdate("Account", ammount.ToString(), user);
        }

        public bool SetUserBankAccount(string ConnectionId, int ammount)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
           int userBankAccount =  GetUserBankAccount(ConnectionId);
            if ((userBankAccount - ammount) < 0)
            {
                ammount = 0;
            }
            return ExecuteUpdate("Account", ammount.ToString(), user);
        }

        public bool SetUserPoints(ServerPlayerModel user, int ammount)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteUpdate("Points", ammount.ToString(), user);
        }

        public bool SetUserPoints(string ConnectionId, int ammount)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteUpdate("Points", ammount.ToString(), user);
        }

        public bool SetUserActivatedStatus(ServerPlayerModel user, bool value)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteUpdate("Activated", (value ? 1 : 0).ToString(), user);
        }

        public bool SetUserActivatedStatus(string ConnectionId, bool value)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteUpdate("Activated", (value ? 1 : 0).ToString(), user);
        }

        public void DeliverPay(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            if(!ServiceProvider.GetPlayerService().Exists(user.SignalRId)) throw new ArgumentException("parameter user is not connected");

            int currentPlayerAccount = GetUserBankAccount(user);
            int salary = ServiceProvider.GetJobService().GetUserJobInfos(user).Salary;
            int newAccount = currentPlayerAccount + salary;
            SetUserBankAccount(user, newAccount);

        }
        public void GetTax(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            if (!ServiceProvider.GetPlayerService().Exists(user.SignalRId)) throw new ArgumentException("parameter user is not connected");

            int currentPlayerAccount = GetUserBankAccount(user);
                int salary = ServiceProvider.GetJobService().GetUserJobInfos(user).Salary;
                int newAccount = currentPlayerAccount - 20;
                SetUserBankAccount(user, newAccount);
           

        }

        public void DeliverPay(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            DeliverPay(user);
        }

        public void DeliverPoints(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            if (!ServiceProvider.GetPlayerService().Exists(user.SignalRId)) throw new ArgumentException("parameter user is not connected");

            int currentPlayerPoints = GetUserPoints(user);
            int points = ServiceProvider.GetJobService().GetUserJobInfos(user).EarningPoints;
            SetUserPoints(user, points + currentPlayerPoints);
        }

        public void DeliverPoints(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            DeliverPoints(user);
        }
    }
}
