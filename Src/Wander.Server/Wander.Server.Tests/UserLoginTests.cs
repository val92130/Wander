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
    public class UserLoginTests
    {
        [TestMethod]
        public void UserLoginWorks()
        {
            TestEnvironment.DeleteTestUser();

            UserAccount user = TestEnvironment.GetTestUserAccount();
            user.Register();

            Assert.IsTrue(user.CheckLogin());

            TestEnvironment.DeleteTestUser();
            
        }

        [TestMethod]
        public void UserLoginWithWrongPasswordReturnsFalse()
        {
            TestEnvironment.DeleteTestUser();

            UserAccount user = TestEnvironment.GetTestUserAccount();
            user.Register();

            UserAccount testUser = new UserAccount();

            testUser.Login = "wrongLogin";
            testUser.Password = "wrongPassword";

            Assert.IsFalse(testUser.CheckLogin());

            TestEnvironment.DeleteTestUser();

        }
    }
}
