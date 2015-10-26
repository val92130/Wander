using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wander.Server.Models;

namespace Wander.Server.Tests
{
    [TestClass]
    public class UserRegistrationTests
    {
        [TestMethod]
        public void UserWithNullFieldsReturnsFalse()
        {
            UserAccount user = new UserAccount();
            user.Login = null;
            user.Password = null;
            user.Email = null;
            user.PasswordConfirm = null;

            Assert.IsFalse(user.CheckRegisterForm());
        }

        [TestMethod]
        public void UserWithOneNullFieldReturnsFalse()
        {
            UserAccount user = new UserAccount();
            user.Login = null;
            user.Password = "pass";
            user.Email = null;
            user.PasswordConfirm = "pass";

            Assert.IsFalse(user.CheckRegisterForm());
        }

        [TestMethod]
        public void UserWithShortPasswordReturnsFalse()
        {
            UserAccount user = new UserAccount();
            user.Login = "user";
            user.Password = "000";
            user.Email = "useremail@provider.fr";
            user.PasswordConfirm = "000";

            Assert.IsFalse(user.CheckRegisterForm());
        }

        [TestMethod]
        public void UserWithShortLoginReturnsFalse()
        {
            UserAccount user = new UserAccount();
            user.Login = "usr";
            user.Password = "pass";
            user.Email = "useremail@provider.fr";
            user.PasswordConfirm = "pass";

            Assert.IsFalse(user.CheckRegisterForm());
        }

        [TestMethod]
        public void UserWithWrongSexReturnsFalse()
        {
            UserAccount user = new UserAccount();
            user.Login = "user";
            user.Password = "pass";
            user.Email = "useremail@provider.fr";
            user.PasswordConfirm = "pass";
            user.Sex = 5;

            Assert.IsFalse(user.CheckRegisterForm());
        }

        [TestMethod]
        public void UserWithIncorrectEmailReturnsFalse()
        {
            UserAccount user = new UserAccount();
            user.Login = "user";
            user.Password = "pass";
            user.Email = "useremail.fr";
            user.PasswordConfirm = "pass";

            Assert.IsFalse(user.CheckRegisterForm());
        }

        [TestMethod]
        public void UserWithCorrectFieldsReturnsTrue()
        {
            TestEnvironment.DeleteTestUser();
            UserAccount user = new UserAccount();
            user.Login = "user";
            user.Password = "pass";
            user.Email = "useremail@provider.fr";
            user.PasswordConfirm = "pass";

            Assert.IsTrue(user.CheckRegisterForm());
        }


        [TestMethod]
        public void CreateUserWorksCorrectly()
        {
            TestEnvironment.DeleteTestUser();
            UserAccount user = TestEnvironment.GetTestUserAccount();

            user.Register();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
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

            UserAccount user = TestEnvironment.GetTestUserAccount();

            user.Register();

            UserAccount user2 = TestEnvironment.GetTestUserAccount();


            Assert.IsTrue(user2.CheckLoginAlreadyExists(user2.Login));

        }
    }
}
