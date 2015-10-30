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
        public void GetPlayerReturnsEveryPlayers()
        {
            ServiceProvider.GetPlayerService().AddPlayer("signId1", 1);
            ServiceProvider.GetPlayerService().AddPlayer("signId2", 2);

            List<ServerPlayerModel> players = ServiceProvider.GetPlayerService().GetAllPlayersServer();

            Assert.AreEqual(players.Count, 2);

            ServerPlayerModel player1 = players.FirstOrDefault(x => (x.SignalRId == "signId1" && x.UserId == 1));
            ServerPlayerModel player2 = players.FirstOrDefault(x => (x.SignalRId == "signId2" && x.UserId == 2));

            Assert.IsNotNull(player1);
            Assert.IsNotNull(player2);
        }


        [TestMethod]
        public void AddPlayerWorksCorrectly()
        {
            ServerPlayerModel serverPlayer = TestEnvironment.GetTestPlayerModel();
            PlayerService playerService = new PlayerService();

            playerService.AddPlayer(serverPlayer.SignalRId, serverPlayer.UserId);

            ServerPlayerModel result = playerService.GetPlayer(serverPlayer.SignalRId);

            Assert.IsTrue((serverPlayer.SignalRId == result.SignalRId && serverPlayer.UserId == result.UserId));
        }

        [TestMethod]
        public void RemovePlayerWorksCorrectly()
        {
            ServerPlayerModel serverPlayer = TestEnvironment.GetTestPlayerModel();
            PlayerService playerService = new PlayerService();

            playerService.AddPlayer(serverPlayer.SignalRId, serverPlayer.UserId);

            playerService.RemovePlayer(serverPlayer.SignalRId);

            Assert.IsNull(playerService.GetPlayer(serverPlayer.SignalRId));
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
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

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
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

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
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

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
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

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
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

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
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

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
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            int JobId = ServiceProvider.GetUserService().GetUserJobId(p);
           
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            Assert.AreEqual(0, JobId);
        }



        [TestMethod]
        public void SetUserBankAccountPlayerModel()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            int initialAccount = ServiceProvider.GetUserService().GetUserBankAccount(p);

            Assert.AreEqual(0, initialAccount);

            int newAmmount = 5200;

            bool flag = ServiceProvider.GetUserService().SetUserBankAccount(p, newAmmount);

            Assert.IsTrue(flag);

            int newAccount = ServiceProvider.GetUserService().GetUserBankAccount(p);

            Assert.AreEqual(newAccount, newAmmount);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void SetUserBankAccountConnectionId()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            int initialAccount = ServiceProvider.GetUserService().GetUserBankAccount("signalrId");

            Assert.AreEqual(0, initialAccount);

            int newAmmount = 5200;

            bool flag = ServiceProvider.GetUserService().SetUserBankAccount("signalrId", newAmmount);

            Assert.IsTrue(flag);

            int newAccount = ServiceProvider.GetUserService().GetUserBankAccount(p);

            Assert.AreEqual(newAccount, newAmmount);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }


        [TestMethod]
        public void SetUserPointsPlayerModel()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            int initialAccount = ServiceProvider.GetUserService().GetUserPoints(p);

            Assert.AreEqual(0, initialAccount);

            int newAmmount = 1650;

            bool flag = ServiceProvider.GetUserService().SetUserPoints(p, newAmmount);

            Assert.IsTrue(flag);

            int newPoints = ServiceProvider.GetUserService().GetUserPoints(p);

            Assert.AreEqual(newPoints, newAmmount);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void SetUserPointsConnectionId()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            int initialAccount = ServiceProvider.GetUserService().GetUserPoints("signalrId");

            Assert.AreEqual(0, initialAccount);

            int newAmmount = 1650;

            bool flag = ServiceProvider.GetUserService().SetUserPoints("signalrId", newAmmount);

            Assert.IsTrue(flag);

            int newPoints = ServiceProvider.GetUserService().GetUserPoints(p);

            Assert.AreEqual(newPoints, newAmmount);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }


        [TestMethod]
        public void SetUserActivatedStatusPlayerModel()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            bool activated = ServiceProvider.GetUserService().GetUserActivatedStatus(p);

            Assert.IsTrue(activated);


            bool flag = ServiceProvider.GetUserService().SetUserActivatedStatus(p, false);

            Assert.IsTrue(flag);

            bool newState = ServiceProvider.GetUserService().GetUserActivatedStatus(p);

            Assert.IsFalse(newState);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void SetUserActivatedStatusConnectionId()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel p = ServiceProvider.GetPlayerService().GetPlayer("signalrId");

            bool activated = ServiceProvider.GetUserService().GetUserActivatedStatus("signalrId");

            Assert.IsTrue(activated);


            bool flag = ServiceProvider.GetUserService().SetUserActivatedStatus("signalrId", false);

            Assert.IsTrue(flag);

            bool newState = ServiceProvider.GetUserService().GetUserActivatedStatus("signalrId");

            Assert.IsFalse(newState);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }


        [TestMethod]
        public void MovePlayerConnectionIdToPositionWorks()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            Vector2 to = new Vector2(50,90);
            ServiceProvider.GetPlayerService().MovePlayerTo("signalrId", to);

            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer("signalrId");
            Assert.IsTrue((player.Position.X == 50 && player.Position.Y == 90));

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void MovePlayerToPositionWorks()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer("signalrId");


            Vector2 to = new Vector2(50, 90);
            ServiceProvider.GetPlayerService().MovePlayerTo(player, to);

            player = ServiceProvider.GetPlayerService().GetPlayer("signalrId");
            Assert.IsTrue((player.Position.X == 50 && player.Position.Y == 90));

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }
    }
}
