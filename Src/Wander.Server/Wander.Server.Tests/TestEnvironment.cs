using System.Data.SqlClient;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server.Tests
{
    public class TestEnvironment
    {
        private static IUserRegistrationService RegistrationService;
        internal static void DeleteTestUser()
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
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

        internal static ServerPlayerModel GetTestPlayerModel()
        {
            return new ServerPlayerModel() {SignalRId = "testId", UserId = 1};
        }

        internal static UserModel GetTestUserModel()
        {
            UserModel user = new UserModel();
            user.Login = "user";
            user.Password = "pass";
            user.Email = "useremail@provider.fr";
            return user;
        }

        internal static IUserRegistrationService GetUserRegistrationService()
        {
            if(RegistrationService == null)
                RegistrationService = new DbUserRegistrationService();

            return RegistrationService;
            
        }

    }
}
