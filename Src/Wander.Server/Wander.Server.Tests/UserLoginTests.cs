using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server.Tests
{
    [TestClass]
    public class UserLoginTests
    {
        [TestMethod]
        public void UserLoginWorks()
        {
            TestEnvironment.DeleteTestUser();

            UserModel user = TestEnvironment.GetTestUserModel();
            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.Register(user);

            Assert.IsTrue(registrationService.CheckLogin(user));

            TestEnvironment.DeleteTestUser();
            
        }

        [TestMethod]
        public void UserLoginWithWrongPasswordReturnsFalse()
        {
            TestEnvironment.DeleteTestUser();

            UserModel user = TestEnvironment.GetTestUserModel();
            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.Register(user);

            UserModel testUser = new UserModel();

            testUser.Login = "wrongLogin";
            testUser.Password = "wrongPassword";

            Assert.IsFalse(registrationService.CheckLogin(testUser));

            TestEnvironment.DeleteTestUser();

        }
        [TestMethod]
        public void UserLoginSetConnectedToOne()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.Register(user);
            registrationService.Connect(user);

            string query = "select * from dbo.Users where UserLogin = @Login";
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", user.Login);
                    var data = cmd.ExecuteReader();
                    while (data.Read())
                    {
                        Assert.AreEqual(data["Connected"], true);
                    }
                    conn.Close();
                }
            }
            TestEnvironment.DeleteTestUser();
        }
    }
}
