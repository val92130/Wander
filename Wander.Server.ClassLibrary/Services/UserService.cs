using System;
using System.Data.SqlClient;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public class UserService : IUserService
    {

        private bool ExecuteUpdate(string field, string value, ServerPlayerModel user)
        {

            if (user == null) throw new ArgumentException("parameter user is null");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("UPDATE dbo.Users SET {0} = {1}  WHERE UserId = @connectionId", field, value);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@connectionId", user.UserId);


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
                string query = string.Format("SELECT {0} from dbo.Users WHERE UserId = @connectionId", value);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@connectionId", user.UserId);


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
                string query = string.Format("SELECT {0} from dbo.Users WHERE UserId = @connectionId", value);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@connectionId", user.UserId);


                    int data = Convert.ToInt32(cmd.ExecuteScalar());

                    conn.Close();

                    return data;
                }
            }
        }

        public string GetUserLogin(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
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

        public string GetUserEmail(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            return ExecuteQuery("Email", user);
        }

        public int GetUserSex(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Sex", user));
        }

        public int GetUserSex(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Sex", user));
        }

        public int GetUserBankAccount(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Account", user));
        }

        public int GetUserBankAccount(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Account", user));
        }

        public int GetUserPoints(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Points", user));
        }

        public int GetUserPoints(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("Points", user));
        }

        public bool GetUserActivatedStatus(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return Convert.ToBoolean(ExecuteQueryInt("Activated", user));
        }

        public bool GetUserActivatedStatus(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return Convert.ToBoolean(this.ExecuteQueryInt("Activated", user));
        }

        public int GetUserJobId(ServerPlayerModel user)
        {

            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("JobId", user));
        }

        public int GetUserJobId(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return (ExecuteQueryInt("JobId", user));
        }

        public ClientPlayerModel GetAllUserInfos(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
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


            int newAmmount = ammount;
            if ((ammount) <= 0)
            {
                newAmmount = 0;
            }
            return ExecuteUpdate("Account", newAmmount.ToString(), user);

        }

        public bool SetUserBankAccount(string connectionId, int ammount)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            int newAmmount = ammount;
            if ((ammount) <= 0)
            {
                newAmmount = 0;
            }
            return ExecuteUpdate("Account", newAmmount.ToString(), user);
        }

        public bool SetUserPoints(ServerPlayerModel user, int ammount)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteUpdate("Points", ammount < 0 ? "0" : ammount.ToString(), user);
        }

        public bool SetUserPoints(string connectionId, int ammount)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return SetUserPoints(user, ammount);
        }

        public bool SetUserActivatedStatus(ServerPlayerModel user, bool value)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteUpdate("Activated", (value ? 1 : 0).ToString(), user);
        }

        public bool SetUserActivatedStatus(string connectionId, bool value)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteUpdate("Activated", (value ? 1 : 0).ToString(), user);
        }

        public void DeliverPay(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            if (!ServiceProvider.GetPlayerService().Exists(user.SignalRId)) throw new ArgumentException("parameter user is not connected");

            int currentPlayerAccount = GetUserBankAccount(user);
            int salary = ServiceProvider.GetJobService().GetUserJobInfos(user).Salary;
            int newAccount = currentPlayerAccount + salary;
            SetUserBankAccount(user, newAccount);


        }

        public void PayTax(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            if (!ServiceProvider.GetPlayerService().Exists(user.SignalRId)) throw new ArgumentException("parameter user is not connected");

            int currentPlayerAccount = GetUserBankAccount(user);
            int salary = ServiceProvider.GetJobService().GetUserJobInfos(user).Salary;
            int newAccount = currentPlayerAccount - (int)(salary * 0.1);
            SetUserBankAccount(user, newAccount);


        }

        public void DeliverPay(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
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

        public void DeliverPoints(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            DeliverPoints(user);
        }

        public bool SetBan(string connectionId, bool value)
        {
            if (connectionId == null) throw new ArgumentException("parameter user is null");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteUpdate("Banned", (value ? "1" : "0"), user);
        }

        public bool SetBan(ServerPlayerModel user, bool value)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return ExecuteUpdate("Banned", (value ? "1" : "0"), user);
        }

        public bool IsBanned(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return Convert.ToBoolean(this.ExecuteQueryInt("Banned", user));
        }

        public bool IsBanned(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return Convert.ToBoolean(this.ExecuteQueryInt("Banned", user));
        }

        public bool IsAdmin(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) return false;
            return Convert.ToBoolean(this.ExecuteQueryInt("Admin", user));
        }

        public bool IsAdmin(ServerPlayerModel user)
        {
            if (user == null) return false;
            return Convert.ToBoolean(this.ExecuteQueryInt("Admin", user));
        }

        public Vector2 GetLastPosition(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            Vector2 pos = new Vector2();
            int x = this.ExecuteQueryInt("LastPosX", user);
            int y = this.ExecuteQueryInt("LastPosY", user);
            pos.X = x;
            pos.Y = y;
            return pos;
        }

        public Vector2 GetLastPosition(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return this.GetLastPosition(user);
        }

        public bool SetLastPosition(ServerPlayerModel user, Vector2 position)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            bool xUpdate =  ExecuteUpdate("LastPosX", Math.Round(position.X).ToString(), user);
            bool yUpdate =  ExecuteUpdate("LastPosY", Math.Round(position.Y).ToString(), user);
            return (xUpdate && yUpdate);
        }

        public bool SetLastPosition(string connectionId, Vector2 position)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return this.SetLastPosition(user, position);
        }

        public int GetRegisteredUsersCount()
        {
            int nbr = -1;
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT COUNT(*) from dbo.Users";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    nbr = (int)cmd.ExecuteScalar();

                    conn.Close();

                }
            }
            return nbr;
        }

        public Vector2 GetLastPosition(int userId)
        {
            return GetLastPosition(new ServerPlayerModel() {UserId = userId});
        }
    }
}
