using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public class DbUserRegistrationService : IUserRegistrationService
    {
        public static int MinPasswordLength = 4;
        public static int MinLoginLength = 4;

        public bool CheckRegisterForm(UserModel user)
        {
            if (user.Login != null && user.Email != null && user.Password != null)
            {
                if (!String.IsNullOrWhiteSpace(user.Login) && !String.IsNullOrWhiteSpace(user.Email) &&
                    !String.IsNullOrWhiteSpace(user.Password))
                {
                    if (user.Login.Length >= MinLoginLength && user.Email.Contains("@") && user.Password.Length >= MinPasswordLength)
                    {
                        if (user.Sex == 1 || user.Sex == 0)
                        {
                            if (!CheckLoginAlreadyExists(user))
                            {
                                return true;
                            }

                        }
                    }
                }
            }
            return false;
        }

        public bool CheckLogin(UserModel user)
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT UserId from dbo.Users WHERE UserLogin = @Login AND UserPassword = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", user.Login);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    var data = cmd.ExecuteReader();
                    bool value = data.HasRows;
                    conn.Close();
                    return value;
                }
            }
        }

        public bool CheckLoginAlreadyExists(UserModel user)
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT UserId from dbo.Users WHERE UserLogin = @Login";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", user.Login);

                    var data = cmd.ExecuteReader();
                    bool value = data.HasRows;
                    conn.Close();
                    return value;
                }
            }
        }

        public void Connect(UserModel user)
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "update dbo.Users set Connected = 1 where UserLogin = @Login";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", user.Login);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public void Register(UserModel user)
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query =
                    "INSERT INTO dbo.Users (UserLogin, Email, UserPassword, Sex, Account, Points, Connected, Activated) values ( @Login, @Email, @Password, @Sex, @Account, @Points, @Connected, @Activated) ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@Login", user.Login);
                    cmd.Parameters.Add("@Email", user.Email);
                    cmd.Parameters.Add("@Password", user.Password);
                    cmd.Parameters.Add("@Sex", user.Sex);
                    cmd.Parameters.AddWithValue("@Account", 0);
                    cmd.Parameters.AddWithValue("@Points", 0);
                    cmd.Parameters.AddWithValue("@Connected", 0);
                    cmd.Parameters.AddWithValue("@Activated", 1);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
        }

        public void LogOut(UserModel user)
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "update dbo.Users set Connected = 0 where UserLogin = @Login";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", user.Login);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

        }
    }
}