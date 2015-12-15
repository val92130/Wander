using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Wander.Server.ClassLibrary.Model.Forms;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

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
        public void NewUserSetPositionToZeroInDb()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();

            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.Register(user);
            int userId = registrationService.Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrUser", userId);
            var lastPos = ServiceProvider.GetUserService().GetLastPosition("signalrUser");
            Assert.AreEqual(lastPos.X, 0);
            Assert.AreEqual(lastPos.Y, 0);


            ServiceProvider.GetUserService().SetLastPosition("signalrUser", new ClassLibrary.Model.Vector2(50, 600));
            lastPos = ServiceProvider.GetUserService().GetLastPosition(userId);
            Assert.AreEqual(lastPos.X, 50);
            Assert.AreEqual(lastPos.Y, 600);

            ServiceProvider.GetPlayerService().RemovePlayer("signalrUser");
            TestEnvironment.DeleteTestUser();

        }


    }
}
