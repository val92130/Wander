using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public class UserService : IUserService
    {

        private bool ExecuteUpdate(string field, string value, int userId)
        {

            if (!UserExists(userId))return false;

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("UPDATE dbo.Users SET {0} = {1}  WHERE UserId = @connectionId", field, value);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@connectionId", userId);


                    int lines = cmd.ExecuteNonQuery();
                    conn.Close();
                    return lines != 0;
                }
            }
        }


        private string ExecuteQuery(string value, int userId)
        {

            if (!UserExists(userId)) return null;

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT {0} from dbo.Users WHERE UserId = @connectionId", value);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@connectionId", userId);


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

        private int ExecuteQueryInt(string value, int userId)
        {

            if (!UserExists(userId)) return -1;

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT {0} from dbo.Users WHERE UserId = @connectionId", value);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@connectionId", userId);


                    int data = Convert.ToInt32(cmd.ExecuteScalar());

                    conn.Close();

                    return data;
                }
            }
        }

        public int GetUserJobId(int userId)
        {
            if (!UserExists(userId)) return -1;
            return (ExecuteQueryInt("JobId", userId));
        }

        public string GetUserLogin(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            return GetUserLogin(user.UserId);
        }

        public string GetUserLogin(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserLogin(user.UserId);
        }

        public string GetUserLogin(int userId)
        {
            if (!UserExists(userId)) return null;
            return ExecuteQuery("UserLogin", userId);
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
            return GetUserEmail(user.UserId);
        }

        public string GetUserEmail(int userId)
        {
            if (!UserExists(userId)) return null;
            return ExecuteQuery("Email", userId);
        }

        public int GetUserBankAccount(int userId)
        {
            if (!UserExists(userId)) return -1;
            return ExecuteQueryInt("Account", userId);
        }

        public string GetUserEmail(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            return GetUserEmail(user.UserId);
        }

        public int GetUserSex(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserSex(user.UserId);
        }

        public int GetUserSex(int userId)
        {
            if (!UserExists(userId)) return -1;
            return ExecuteQueryInt("Sex", userId);
        }

        public int GetUserPoints(int userId)
        {
            if (!UserExists(userId)) return -1;
            return ExecuteQueryInt("Points", userId);
        }

        public int GetUserSex(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserSex(user.UserId);
        }

        public int GetUserBankAccount(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserBankAccount(user.UserId);
        }

        public bool GetUserActivatedStatus(int userId)
        {
            if (!UserExists(userId))return false;
            return Convert.ToBoolean(ExecuteQueryInt("Activated", userId));
        }

        public int GetUserBankAccount(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserBankAccount(user.UserId);
        }

        public int GetUserPoints(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserPoints(user.UserId);
        }

        public int GetUserPoints(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserPoints(user.UserId);
        }

        public bool GetUserActivatedStatus(ServerPlayerModel user)
        {
            return GetUserActivatedStatus(user.UserId);
        }

        public bool GetUserActivatedStatus(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserActivatedStatus(user.UserId);
        }

        public int GetUserJobId(ServerPlayerModel user)
        {

            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserJobId(user.UserId);
        }

        public int GetUserJobId(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserJobId(user.UserId);
        }

        public void DeliverPoints(int userId)
        {
            if (!UserExists(userId)) throw new ArgumentException("parameter user is null");

            int currentPlayerPoints = GetUserPoints(userId);
            int points = ServiceProvider.GetJobService().GetUserJobInfos(userId).EarningPoints;
            SetUserPoints(userId, points + currentPlayerPoints);
        }

        public ClientPlayerModel GetAllUserInfos(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetAllUserInfos(user.UserId);

        }

        public ClientPlayerModel GetAllUserInfos(int userId)
        {
            if (!UserExists(userId)) return null;

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT * FROM USERS WHERE UserId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Id", userId);

                    ClientPlayerModel client = new ClientPlayerModel();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        client.Sex = Convert.ToInt32(reader["Sex"]);
                        client.Account = Convert.ToInt32(reader["Account"]);
                        client.Points = Convert.ToInt32(reader["Points"]);
                        client.UserName = reader["UserLogin"].ToString();
                        client.Email = reader["Email"].ToString();
                        client.Job = ServiceProvider.GetJobService().GetUserJobInfos(userId);
                        break;
                    }

                    conn.Close();

                    return client;
                }
            }

        }

        public bool SetUserBankAccount(ServerPlayerModel user, int ammount)
        {
            return SetUserBankAccount(user.UserId, ammount);

        }

        public bool SetUserBankAccount(int userId, int ammount)
        {
            if (!UserExists(userId))return false;


            int newAmmount = ammount;
            if ((ammount) <= 0)
            {
                newAmmount = 0;
            }
            return ExecuteUpdate("Account", newAmmount.ToString(), userId);
        }

        public bool SetUserActivatedStatus(int userId, bool value)
        {
            if (!UserExists(userId)) return false;
            return ExecuteUpdate("Activated", (value ? 1 : 0).ToString(), userId);
        }

        public bool SetUserBankAccount(string connectionId, int ammount)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            return SetUserBankAccount(user.UserId, ammount);
        }

        public bool SetUserPoints(ServerPlayerModel user, int ammount)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return SetUserPoints(user.UserId, ammount);
        }

        public bool SetUserPoints(int userId, int ammount)
        {
            if (!UserExists(userId)) return false;
            return ExecuteUpdate("Points", ammount < 0 ? "0" : ammount.ToString(), userId);
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
            return SetUserActivatedStatus(user.UserId, value);
        }

        public bool SetBan(int userId, bool value)
        {
            if (!UserExists(userId)) return false;
            return ExecuteUpdate("Banned", (value ? "1" : "0"), userId);
        }

        public bool SetUserActivatedStatus(string connectionId, bool value)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return SetUserActivatedStatus(user.UserId, value);
        }

        public void DeliverPay(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");

            DeliverPay(user.UserId);

        }

        public void DeliverPay(int userId)
        {
            if (!UserExists(userId))return;
            int currentPlayerAccount = GetUserBankAccount(userId);
            int salary = ServiceProvider.GetJobService().GetUserJobInfos(userId).Salary;
            int newAccount = currentPlayerAccount + salary;
            SetUserBankAccount(userId, newAccount);
        }

        public bool IsBanned(int userId)
        {
            if (!UserExists(userId)) return false;
            return Convert.ToBoolean(this.ExecuteQueryInt("Banned", userId));
        }

        public void PayTax(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            PayTax(user.UserId);


        }

        public void PayTax(int userId)
        {
            if (!UserExists(userId)) return;
            if (!ServiceProvider.GetPlayerService().Exists(userId)) throw new ArgumentException("parameter user is not connected");

            int currentPlayerAccount = GetUserBankAccount(userId);
            int salary = ServiceProvider.GetJobService().GetUserJobInfos(userId).Salary;
            int newAccount = currentPlayerAccount - (int)(salary * 0.1);
            SetUserBankAccount(userId, newAccount);
        }

        public bool UserExists(int userId)
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT UserId from dbo.Users WHERE UserId = @connectionId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@connectionId", userId);


                    var data = cmd.ExecuteReader();
                    bool val = data.HasRows;

                    conn.Close();

                    return val;
                }
            }
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
            DeliverPoints(user.UserId);
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
            return SetBan(user.UserId, value);
        }

        public bool SetBan(ServerPlayerModel user, bool value)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return SetBan(user.UserId, value);
        }

        public bool IsBanned(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return IsBanned(user.UserId);
        }

        public bool IsBanned(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return IsBanned(user.UserId);
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
            return SetLastPosition(user.UserId, position);
        }

        public bool SetLastPosition(string connectionId, Vector2 position)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");
            return this.SetLastPosition(user.UserId, position);
        }

        public bool SetLastPosition(int userId, Vector2 position)
        {
            if (!UserExists(userId))return false;
            bool xUpdate = ExecuteUpdate("LastPosX", Math.Round(position.X).ToString(), userId);
            bool yUpdate = ExecuteUpdate("LastPosY", Math.Round(position.Y).ToString(), userId);
            return (xUpdate && yUpdate);
        }

        public List<int> GetAllUsersId()
        {
            List<int> usersId = new List<int>();
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT UserId from dbo.Users ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    var data = cmd.ExecuteReader();
                    while (data.Read())
                    {
                        int id = (int)(data["UserId"]);
                        usersId.Add(id);
                    }
                    data.Close();

                    conn.Close();

                    return usersId;
                }
            }
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
