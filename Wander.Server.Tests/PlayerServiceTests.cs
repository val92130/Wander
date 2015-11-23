using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server.Tests
{
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

            playerService.RemovePlayer(serverPlayer.SignalRId);

            TestEnvironment.DeleteTestUser();
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
            ServiceProvider.GetPlayerService().MovePlayerTo("signalrId", to, Model.Players.EPlayerDirection.Idle);

            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer("signalrId");
            Assert.IsTrue((player.Position.X == 50 && player.Position.Y == 90));
            Assert.IsTrue(player.Direction == Model.Players.EPlayerDirection.Idle);

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
            ServiceProvider.GetPlayerService().MovePlayerTo(player, to, Model.Players.EPlayerDirection.Idle);

            player = ServiceProvider.GetPlayerService().GetPlayer("signalrId");
            Assert.IsTrue((player.Position.X == 50 && player.Position.Y == 90));
            Assert.IsTrue(player.Direction == Model.Players.EPlayerDirection.Idle);

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void ChangePlayerDirectionWorks()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer("signalrId");
            Assert.IsTrue(player.Direction == Model.Players.EPlayerDirection.Idle);

            Vector2 to = new Vector2(50, 90);
            ServiceProvider.GetPlayerService().MovePlayerTo(player, to, Model.Players.EPlayerDirection.DownRight);

            player = ServiceProvider.GetPlayerService().GetPlayer("signalrId");
            Assert.IsTrue((player.Position.X == 50 && player.Position.Y == 90));
            Assert.IsTrue(player.Direction == Model.Players.EPlayerDirection.DownRight);

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void TestPlayerExistReturnFalseIfTheUserDoesntExist()
        {
            Assert.IsFalse(ServiceProvider.GetPlayerService().Exists("randomId"));
        }

        [TestMethod]
        public void TestPlayerExistReturnTrueIfTheUserExist()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            Assert.IsTrue(ServiceProvider.GetPlayerService().Exists("signalrId"));
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetAllPlayersConnectionIdWorks()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            UserModel user2 = new UserModel() {Login = "user2", Email = "tt@mail.com", Password = "1234", Sex = 0};
            ServiceProvider.GetUserRegistrationService().Register(user2);
            int id2 = ServiceProvider.GetUserRegistrationService().Connect(user2);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId2", id2);

            Assert.IsTrue(ServiceProvider.GetPlayerService().GetAllPlayersConnectionId().Contains("signalrId"));
            Assert.IsTrue(ServiceProvider.GetPlayerService().GetAllPlayersConnectionId().Contains("signalrId2"));


            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId2");

            TestEnvironment.DeleteTestUser();

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                conn.Open();
                string deleteQuery = "DELETE FROM dbo.Users WHERE UserLogin = @User ";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@User", "user2");
                    deleteCmd.ExecuteNonQuery();
                }

                conn.Close();
            }

        }

        [TestMethod]
        public void GetPlayerInfoWorks()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            ClientPlayerModel model = ServiceProvider.GetPlayerService().GetPlayerInfos("signalrId");

            Assert.IsNotNull(model);
            Assert.AreEqual(model.UserName, user.Login);
            Assert.AreEqual(model.Sex, user.Sex);
            Assert.AreEqual(model.Email, user.Email);
            Assert.IsTrue(model.Properties.Count == 0);
            Assert.AreEqual(model.Job.JobDescription, "unemployed");

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");

            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetAllPlayersClientWorks()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            UserModel user2 = new UserModel() { Login = "user2", Email = "tt@mail.com", Password = "1234", Sex = 0 };
            ServiceProvider.GetUserRegistrationService().Register(user2);
            int id2 = ServiceProvider.GetUserRegistrationService().Connect(user2);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId2", id2);

            List<ClientPlayerModel> clients = ServiceProvider.GetPlayerService().GetAllPlayersClient();


            Assert.IsTrue(clients.FirstOrDefault(x => x.UserName == "user") != null);
            Assert.IsTrue(clients.FirstOrDefault(x => x.UserName == "user2") != null);


            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId2");

            TestEnvironment.DeleteTestUser();

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                conn.Open();
                string deleteQuery = "DELETE FROM dbo.Users WHERE UserLogin = @User ";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@User", "user2");
                    deleteCmd.ExecuteNonQuery();
                }

                conn.Close();
            }

        }



        [TestMethod]
        public void DeliverPayWorks()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            int oldAccount = ServiceProvider.GetUserService().GetUserBankAccount("signalrId");


            int newJobId =
                ServiceProvider.GetJobService()
                    .AddJob(new JobModel() { JobDescription = "Des1", Threshold = 10, Salary = 2000, EarningPoints = 100, NecessaryPoints = 0 });


            JobModel job = ServiceProvider.GetJobService().GetAllJobs().FirstOrDefault(x => x.JobId == newJobId);

            ServiceProvider.GetJobService().ChangeUserJob(job.JobId, "signalrId");
            ServiceProvider.GetUserService().DeliverPay("signalrId");

            int newAccount = ServiceProvider.GetUserService().GetUserBankAccount("signalrId");

            Assert.AreEqual(newAccount, oldAccount + job.Salary);

            ServiceProvider.GetJobService().ChangeUserJob(0, "signalrId");
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            ServiceProvider.GetJobService().DeleteJob(job);
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void DeliveryPointsWorks()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);

            int oldPoints = ServiceProvider.GetUserService().GetUserPoints("signalrId");


            int newJobId =
                ServiceProvider.GetJobService()
                    .AddJob(new JobModel() { JobDescription = "Des1", Threshold = 10, Salary = 2000, EarningPoints = 100, NecessaryPoints = 0 });


            JobModel job = ServiceProvider.GetJobService().GetAllJobs().FirstOrDefault(x => x.JobId == newJobId);

            ServiceProvider.GetJobService().ChangeUserJob(job.JobId, "signalrId");
            ServiceProvider.GetUserService().DeliverPoints("signalrId");

            int newPoints = ServiceProvider.GetUserService().GetUserPoints("signalrId");

            Assert.AreEqual(newPoints, oldPoints + job.EarningPoints);

            ServiceProvider.GetJobService().ChangeUserJob(0, "signalrId");
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            ServiceProvider.GetJobService().DeleteJob(job);
            TestEnvironment.DeleteTestUser();
        }


    }
}
