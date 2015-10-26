using System;
using System.Collections.Generic;
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
            UserAccount user = new UserAccount();
            user.Login = "user";
            user.Password = "pass";
            user.Email = "useremail@provider.fr";
            user.PasswordConfirm = "pass";

            Assert.IsTrue(user.CheckRegisterForm());
        }
    }
}
