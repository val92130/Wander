using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Wander.Server.Models
{
    public class UserAccount
    {
        static int minPasswordLength = 4;
        static int minLoginLength = 4;
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
                    if (Login.Length >= minLoginLength && Email.Contains("@") && Password.Length >= minPasswordLength && Password == PasswordConfirm)
                    {
                        if (Sex == 1 || Sex == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckLogin()
        {
            // TO DO : CHECK USER CREDENTIALS
            return false;
        }
        public void Register()
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string query = "INSERT INTO dbo.Users ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    conn.Close();
                }
            // TO DO : REGISTER USER INTO BDD
        }
    }
}