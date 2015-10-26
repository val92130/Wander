using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.Models;

namespace Wander.Server.Tests
{
    public class TestEnvironment
    {
        internal static void DeleteTestUser()
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string deleteQuery = "DELETE FROM dbo.Users WHERE UserLogin = @User AND UserPassword = @Password ";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@User", "user");
                    deleteCmd.Parameters.AddWithValue("@Password", "pass");
                    deleteCmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        internal static UserAccount GetTestUserAccount()
        {
            UserAccount user = new UserAccount();
            user.Login = "user";
            user.Password = "pass";
            user.Email = "useremail@provider.fr";
            user.PasswordConfirm = "pass";
            return user;
        }

    }
}
