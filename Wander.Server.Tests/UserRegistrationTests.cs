using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wander.Server.Model;
using Wander.Server.Services;
using System;

namespace Wander.Server.Tests
{
    [TestClass]
    public class UserRegistrationTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullUserRegistrationThrowsException()
        {
            UserModel user = null;
            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.Register(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullUserLoginCheckThrowsException()
        {
            UserModel user = null;
            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.CheckLogin(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullUserFormCheckThrowsException()
        {
            UserModel user = null;
            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.CheckRegisterForm(user);
        }

        [TestMethod]
        public void UserWithNullFieldsReturnsFalse()
        {
            UserModel user = new UserModel();
            user.Login = null;
            user.Password = null;
            user.Email = null;

            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();

            Assert.IsFalse(registrationService.CheckRegisterForm(user));
        }

        [TestMethod]
        public void UserWithOneNullFieldReturnsFalse()
        {
            UserModel user = new UserModel();
            user.Login = null;
            user.Password = "pass";
            user.Email = null;

            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();

            Assert.IsFalse(registrationService.CheckRegisterForm(user));
        }

        [TestMethod]
        public void UserWithShortPasswordReturnsFalse()
        {
            UserModel user = new UserModel();
            user.Login = "user";
            user.Password = "000";
            user.Email = "useremail@provider.fr";

            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();

            Assert.IsFalse(registrationService.CheckRegisterForm(user));
        }

        [TestMethod]
        public void UserWithShortLoginReturnsFalse()
        {
            UserModel user = new UserModel();
            user.Login = "usr";
            user.Password = "pass";
            user.Email = "useremail@provider.fr";

            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();

            Assert.IsFalse(registrationService.CheckRegisterForm(user));
        }

        [TestMethod]
        public void UserWithWrongSexReturnsFalse()
        {
            UserModel user = new UserModel();
            user.Login = "user";
            user.Password = "pass";
            user.Email = "useremail@provider.fr";
            user.Sex = 5;

            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();

            Assert.IsFalse(registrationService.CheckRegisterForm(user));
        }

        [TestMethod]
        public void UserWithIncorrectEmailReturnsFalse()
        {
            UserModel user = new UserModel();
            user.Login = "user";
            user.Password = "pass";
            user.Email = "useremail.fr";

            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();

            Assert.IsFalse(registrationService.CheckRegisterForm(user));
        }

        [TestMethod]
        public void UserWithCorrectFieldsReturnsTrue()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = new UserModel();
            user.Login = "user";
            user.Password = "pass";
            user.Email = "useremail@provider.fr";

            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();

            Assert.IsTrue(registrationService.CheckRegisterForm(user));
        }


        [TestMethod]
        public void CreateUserWorksCorrectly()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();


            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.Register(user);

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM dbo.Users WHERE UserLogin = @User AND UserPassword = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@User", "user");
                    cmd.Parameters.AddWithValue("@Password", "pass");
                    var data = cmd.ExecuteReader();

                    while (data.Read())
                    {
                        Assert.AreEqual(data["UserLogin"], "user");
                        Assert.AreEqual(data["UserPassword"], "pass");
                        Assert.AreEqual(data["Email"], "useremail@provider.fr");
                        Assert.AreEqual(data["Account"], 0);
                        Assert.AreEqual(data["Points"], 0);
                        Assert.AreEqual(data["Connected"], false);
                        Assert.AreEqual(data["Activated"], true);
                    }

                    data.Close();
                }

                TestEnvironment.DeleteTestUser();

                conn.Close();
            }
        }


        [TestMethod]
        public void CreateUserAlreadyExistThrowError()
        {
            TestEnvironment.DeleteTestUser();

            UserModel user = TestEnvironment.GetTestUserModel();
            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.Register(user);

            UserModel user2 = TestEnvironment.GetTestUserModel();


            Assert.IsTrue(registrationService.CheckLoginAlreadyExists(user2));

            TestEnvironment.DeleteTestUser();

        }

        [TestMethod]
        public void CreateUserEmailAlreadyExistThrowError()
        {
            TestEnvironment.DeleteTestUser();

            UserModel user = TestEnvironment.GetTestUserModel();
            IUserRegistrationService registrationService = TestEnvironment.GetUserRegistrationService();
            registrationService.Register(user);

            UserModel user2 = TestEnvironment.GetTestUserModel();


            Assert.IsTrue(registrationService.CheckEmailAlreadyExists(user2));

            TestEnvironment.DeleteTestUser();

        }
    }
}
