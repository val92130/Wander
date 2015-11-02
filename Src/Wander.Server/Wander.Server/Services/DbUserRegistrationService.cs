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

        /// <summary>
        /// Checks whether the UserModel fields matches the condition for the form validation
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns true if the UserModel is valid, otherwise returns false</returns>
        public bool CheckRegisterForm(UserModel user)
        {
            if (user == null)
                throw new ArgumentException("parameter user is null");

            if (user == null)
                return false;
            if (user.Login != null && user.Email != null && user.Password != null)
            {
                if (!String.IsNullOrWhiteSpace(user.Login) && !String.IsNullOrWhiteSpace(user.Email) &&
                    !String.IsNullOrWhiteSpace(user.Password) && Helper.IsValidEmail(user.Email))
                {
                    if (user.Login.Length >= MinLoginLength && user.Email.Contains("@") && user.Password.Length >= MinPasswordLength)
                    {
                        if (user.Sex == 1 || user.Sex == 0)
                        {
                            if (!CheckLoginAlreadyExists(user) && !CheckEmailAlreadyExists(user))
                            {
                                return true;
                            }

                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the password and login from the UserModel correspond to a User in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns true if the login and password correspond to a User, otherwise returns false</returns>
        public bool CheckLogin(UserModel user)
        {
            if (user == null)
                throw new ArgumentException("parameter user is null");

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

        /// <summary>
        /// Check if the provided UserModel's login already exists in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns true if the UserModel's login already exists, otherwise return false</returns>
        public bool CheckLoginAlreadyExists(UserModel user)
        {
            if (user == null)
                throw new ArgumentException("parameter user is null");

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

        /// <summary>
        /// Check if the provided UserModel's email already exists in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns true if the UserModel's email already exists, otherwise return false</returns>
        public bool CheckEmailAlreadyExists(UserModel user)
        {
            if (user == null)
                throw new ArgumentException("parameter user is null");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT UserId from dbo.Users WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Email", user.Email);

                    var data = cmd.ExecuteReader();
                    bool value = data.HasRows;
                    conn.Close();
                    return value;
                }
            }
        }

        /// <summary>
        /// Change the Connected state of the provided UserModel to 1
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns the UserId of the user if it exists, otherwise returns -1</returns>
        public int Connect(UserModel user)
        {
            if (user == null)
                throw new ArgumentException("parameter user is null, cannot connect");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "update dbo.Users set Connected = 1 where UserLogin = @Login";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", user.Login);
                    cmd.ExecuteNonQuery();
                }
                

                query = "select UserId from dbo.Users WHERE UserLogin = @Login";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@Login", user.Login);
                    var data = cmd.ExecuteReader();

                    while (data.Read())
                    {
                        return Int32.Parse(data["UserId"].ToString());
                    }
                }
                conn.Close();
                return -1;               
            }
        }

        /// <summary>
        /// Register the UserModel in the database
        /// </summary>
        /// <param name="user"></param>
        public void Register(UserModel user)
        {
            if (user == null)
                throw new ArgumentException("parameter user is null, cannot register");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query =
                    "INSERT INTO dbo.Users (UserLogin, Email, UserPassword, Sex, Account, Points, Connected, Activated) values ( @Login, @Email, @Password, @Sex, @Account, @Points, @Connected, @Activated) ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", user.Login);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Sex", user.Sex);
                    cmd.Parameters.AddWithValue("@Account", 0);
                    cmd.Parameters.AddWithValue("@Points", 0);
                    cmd.Parameters.AddWithValue("@Connected", 0);
                    cmd.Parameters.AddWithValue("@Activated", 1);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Change the Connected state of the provided UserModel to 0
        /// </summary>
        /// <param name="user"></param>
        public void LogOut(UserModel user)
        {
            if (user == null)
                throw new ArgumentException("parameter user is null, cannot log him out");
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

        /// <summary>
        /// Change the Connected state of the provided ServerPlayerModel to 0
        /// </summary>
        /// <param name="user"></param>
        public void LogOut(ServerPlayerModel user)
        {
            if (user == null)
                throw new ArgumentException("parameter user is null, cannot log him out");
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "update dbo.Users set Connected = 0 where UserId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Id", user.UserId);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
    }
}