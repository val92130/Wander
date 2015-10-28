using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server.Tests
{
    using System.Diagnostics;

    [TestClass]
    public class PlayerServiceTests
    {
        [TestMethod]
        public void AddPlayerWorksCorrectly()
        {
            PlayerModel player = TestEnvironment.GetTestPlayerModel();
            PlayerService playerService = new PlayerService();

            playerService.AddPlayer(player.SignalRId, player.UserId);

            PlayerModel result = playerService.GetPlayer(player.SignalRId);

            Assert.IsTrue((player.SignalRId == result.SignalRId && player.UserId == result.UserId));
        }

        [TestMethod]
        public void RemovePlayerWorksCorrectly()
        {
            PlayerModel player = TestEnvironment.GetTestPlayerModel();
            PlayerService playerService = new PlayerService();

            playerService.AddPlayer(player.SignalRId, player.UserId);

            playerService.RemovePlayer(player.SignalRId);

            Assert.IsNull(playerService.GetPlayer(player.SignalRId));
        }

        [TestMethod]
        public void GetUserLoginWithConnectionId()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            string login = ServiceProvider.GetUserService().GetUserLogin("signalrId");
            Console.WriteLine(user.Login + " " + login);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(user.Login, login);
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetUserLoginPlayerModelUser()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            PlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            string login = ServiceProvider.GetUserService().GetUserLogin(p);
            Console.WriteLine(user.Login + " " + login);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(user.Login, login);
        }

        [TestMethod]
        public void GetUserEmailWithConnectionId()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            string Email = ServiceProvider.GetUserService().GetUserEmail("signalrId");
            Console.WriteLine(user.Email + " " + Email);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(user.Email, Email);
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetUserEmailPlayerModelUser()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            PlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            string Email = ServiceProvider.GetUserService().GetUserEmail(p);
            Console.WriteLine(user.Email + " " + Email);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(user.Email, Email);
        }

        [TestMethod]
        public void GetUserSexWithConnectionId()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            int Sex = ServiceProvider.GetUserService().GetUserSex("signalrId");
            Console.WriteLine(user.Sex + " " + Sex);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(user.Sex, Sex);
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetUserSexPlayerModelUser()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            PlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            int Sex = ServiceProvider.GetUserService().GetUserSex(p);
            Console.WriteLine(user.Sex + " " + Sex);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(user.Sex, Sex);
        }



        [TestMethod]
        public void GetUserBankAccountWithConnectionId()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            int Account = ServiceProvider.GetUserService().GetUserBankAccount("signalrId");
            Console.WriteLine(0 + Account);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(0, Account);
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetUserBankAccountPlayerModelUser()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            PlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            int Account = ServiceProvider.GetUserService().GetUserBankAccount(p);
            Console.WriteLine(0 + " " + Account);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(0, Account);
        }

        [TestMethod]
        public void GetUserPointsWithConnectionId()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            int Points = ServiceProvider.GetUserService().GetUserPoints("signalrId");
            Console.WriteLine(0 + Points);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(0, Points);
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetUserPointsPlayerModelUser()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            PlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            int Points = ServiceProvider.GetUserService().GetUserPoints(p);
            Console.WriteLine(0 + " " + Points);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(0, Points);
        }

        [TestMethod]
        public void GetUserActivatedStatutsWithConnectionId()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            bool Activated = ServiceProvider.GetUserService().GetUserActivatedStatus("signalrId");

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(true, Activated);
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetUserActivatedStatutPlayerModelUser()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            PlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            bool Activated = ServiceProvider.GetUserService().GetUserActivatedStatus(p);
             
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(true, Activated);

            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetUserJobIdWithConnectionId()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            int JobId = ServiceProvider.GetUserService().GetUserJobId("signalrId");
        
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(0, JobId);
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetUserJobIdPlayerModelUser()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            PlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            int JobId = ServiceProvider.GetUserService().GetUserJobId(p);
           
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(0, JobId);
        }
    }
}
