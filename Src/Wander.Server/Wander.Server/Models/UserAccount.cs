using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Wander.Server.Models
{
    public class UserAccount
    {
        public static int MinPasswordLength = 4;
        public static int MinLoginLength = 4;
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public int Sex { get; set; }

        public bool CheckRegisterForm()
        {
            if (Login != null && Email != null && Password != null && PasswordConfirm != null && Sex != null)
            {
                if (!String.IsNullOrWhiteSpace(Login) && !String.IsNullOrWhiteSpace(Email) &&
                    !String.IsNullOrWhiteSpace(Password) && !String.IsNullOrWhiteSpace(PasswordConfirm))
                {
                    if (Login.Length >= MinLoginLength && Email.Contains("@") && Password.Length >= MinPasswordLength &&
                        Password == PasswordConfirm)
                    {
                        if (Sex == 1 || Sex == 0)
                        {
                            if (!CheckLoginAlreadyExists(Login))
                            {
                                return true;
                            }
                            
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckLogin()
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = "SELECT UserId from dbo.Users WHERE UserLogin = @Login AND UserPassword = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", Login);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    var data = cmd.ExecuteReader();
                    bool value = data.HasRows;
                    conn.Close();
                    return value;
                }
            }
        }

        public bool CheckLoginAlreadyExists(string login)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = "SELECT UserId from dbo.Users WHERE UserLogin = @Login";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", login);

                    var data = cmd.ExecuteReader();
                    bool value = data.HasRows;
                    conn.Close();
                    return value;
                }
            }
        }

        public void Register()
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query =
                    "INSERT INTO dbo.Users (UserLogin, Email, UserPassword, Sex, Account, Points, Connected, Activated) values ( @Login, @Email, @Password, @Sex, @Account, @Points, @Connected, @Activated) ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@Login", Login);
                    cmd.Parameters.Add("@Email", Email);
                    cmd.Parameters.Add("@Password", Password);
                    cmd.Parameters.Add("@Sex", Sex);
                    cmd.Parameters.AddWithValue("@Account", 0);
                    cmd.Parameters.AddWithValue("@Points", 0);
                    cmd.Parameters.AddWithValue("@Connected", 0);
                    cmd.Parameters.AddWithValue("@Activated", 1);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
        }
    }
}