using System.Data.SqlClient;
using Wander.Server.ClassLibrary.Model.Forms;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.Tests
{
    using System;


    public class TestEnvironment
    {
        private static IUserRegistrationService RegistrationService;
        internal static void DeleteTestUser()
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                conn.Open();
                string deleteQuery = "DELETE FROM dbo.Users WHERE UserLogin = @User";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@User", "user");
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
        internal static void AddPropertyTest(ServerPropertyModel property)
        {
            if (property == null)
                throw new ArgumentException("parameter property is null, cannot register");
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query =
                    "INSERT INTO dbo.ListProperties (NameProperty, PropertyDescription, Threshold, Price) values (@NameProperty, @PropertyDescription, @Threshold, @Price) ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@NameProperty", property.PropertyName);
                    cmd.Parameters.AddWithValue("@PropertyDescription", property.PropertyDescription);
                    cmd.Parameters.AddWithValue("@Threshold", 5);
                    cmd.Parameters.AddWithValue("@Price", 1000);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
        }
        internal static ServerPropertyModel GetTestPropertyModel()
        {
            ServerPropertyModel property = new ServerPropertyModel();
            property.PropertyName = "barTest";
            property.Price = 1000;
            property.Threshold = 1;
            property.PropertyDescription = "nice big bar, contact to nigociate";
            return property;
        }
        internal static void DeletePropertyTest()
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                conn.Open();
                string deleteQuery = "DELETE FROM dbo.ListProperties WHERE NameProperty = @Property";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@Property", "barTest");
                    deleteCmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }
    }
}
