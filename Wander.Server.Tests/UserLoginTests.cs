using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server.Tests
{
    [TestClass]
    public class UserLoginTests
    {
        [TestCleanup()]
        public void Cleanup()
        {
            TestEnvironment.DeleteTestUser();
        }

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

        [TestMethod]
        public void UserLogoutSetConnectedToFalse()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();

            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.Register(user);
            registrationService.Connect(user);

            registrationService.LogOut(user);

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
                        Assert.AreEqual(data["Connected"], false);
                    }
                    conn.Close();
                }
            }
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void UserLogoutSetConnectedToFalseWithPlayerModel()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();

            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.Register(user);
            int userId = registrationService.Connect(user);

            ServerPlayerModel serverPlayer = new ServerPlayerModel() { UserId = userId };
            registrationService.LogOut(serverPlayer);

            string query = "select * from dbo.Users where UserId = @Id";
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Id", serverPlayer.UserId);
                    var data = cmd.ExecuteReader();
                    while (data.Read())
                    {
                        Assert.AreEqual(data["Connected"], false);
                    }
                    conn.Close();
                }
            }
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void IncorrectEmailReturnsFalse()
        {
            string incorrectEmail = "ttttt.fr";
            Assert.IsFalse(Helper.IsValidEmail(incorrectEmail));
        }

        [TestMethod]
        public void CorrectEmailReturnTrue()
        {
            string correctEmail = "val@live.fr";
            Assert.IsTrue(Helper.IsValidEmail(correctEmail));
        }

        [TestMethod]

        public void BanUserWorksCorrectly()
        {
            TestEnvironment.DeleteTestUser();

            UserModel u = TestEnvironment.GetTestUserModel();

            ServiceProvider.GetUserRegistrationService().Register(u);
            int id = ServiceProvider.GetUserRegistrationService().Connect(u);

            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            Assert.IsFalse(ServiceProvider.GetUserService().IsBanned("signalrId"));           

            ServiceProvider.GetUserService().SetBan("signalrId", true);

            Assert.IsTrue(ServiceProvider.GetUserService().IsBanned("signalrId"));

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");

            TestEnvironment.DeleteTestUser();

        }

    }
}
