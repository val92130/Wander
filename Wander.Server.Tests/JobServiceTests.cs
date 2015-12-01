using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wander.Server.ClassLibrary;
using Wander.Server.ClassLibrary.Model.Forms;
using Wander.Server.ClassLibrary.Model.Job;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.Tests
{

    [TestClass]
    public class JobServiceTests
    {
        [TestCleanup()]
        public void Cleanup()
        {
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetUserJobServerModelWorks()
        {
            TestEnvironment.DeleteTestUser();

            UserModel u = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(u);
            int id = ServiceProvider.GetUserRegistrationService().Connect(u);

            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer(id);

            JobModel model = ServiceProvider.GetJobService().GetUserJobInfos(player);

           

            Assert.AreEqual(model.JobDescription, "unemployed");
            Assert.AreEqual(model.JobId, 0);
            Assert.AreEqual(model.Salary, 0);
            Assert.AreEqual(model.Threshold, 0);
            Assert.AreEqual(model.EarningPoints, GameManager.DefaultUnemployedEarningPoints);
            Assert.AreEqual(model.NecessaryPoints, 0);

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetUserJobConnectionIdWorks()
        {
            TestEnvironment.DeleteTestUser();

            UserModel u = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(u);
            int id = ServiceProvider.GetUserRegistrationService().Connect(u);

            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer(id);

            JobModel model = ServiceProvider.GetJobService().GetUserJobInfos("signalrId");

            Assert.AreEqual(model.JobDescription, "unemployed");
            Assert.AreEqual(model.JobId, 0);
            Assert.AreEqual(model.Salary, 0);
            Assert.AreEqual(model.Threshold, 0);
            Assert.AreEqual(model.EarningPoints, GameManager.DefaultUnemployedEarningPoints);
            Assert.AreEqual(model.NecessaryPoints, 0);

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();

        }

        [TestMethod]
        public void ChangerUserJobEnoughPointsServerModelWorks()
        {
            TestEnvironment.DeleteTestUser();

            UserModel u = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(u);
            int id = ServiceProvider.GetUserRegistrationService().Connect(u);

            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer(id);

            JobModel model = ServiceProvider.GetJobService().GetUserJobInfos("signalrId");

            Assert.AreEqual(model.JobDescription, "unemployed");
            Assert.AreEqual(model.JobId, 0);
            Assert.AreEqual(model.Salary, 0);
            Assert.AreEqual(model.Threshold, 0);
            Assert.AreEqual(model.EarningPoints, GameManager.DefaultUnemployedEarningPoints);
            Assert.AreEqual(model.NecessaryPoints, 0);

            int newJobId =
                ServiceProvider.GetJobService()
                    .AddJob(new JobModel() { JobDescription = "Des1", Threshold = 10, Salary = 2000 });

            ServiceProvider.GetJobService().ChangeUserJob(newJobId, player);

            JobModel newJobModel = ServiceProvider.GetJobService().GetUserJobInfos("signalrId");

            Assert.AreEqual(newJobModel.JobDescription, "Des1");
            Assert.AreEqual(newJobModel.JobId, newJobId);
            Assert.AreEqual(newJobModel.Salary, 2000);
            Assert.AreEqual(newJobModel.Threshold, 10);

            ServiceProvider.GetJobService().ChangeUserJob(0, player);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");         
            ServiceProvider.GetJobService().DeleteJob(newJobId);
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void ChangerUserJobNotEnoughPoints()
        {
            TestEnvironment.DeleteTestUser();

            UserModel u = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(u);
            int id = ServiceProvider.GetUserRegistrationService().Connect(u);

            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer(id);

            JobModel model = ServiceProvider.GetJobService().GetUserJobInfos("signalrId");

            Assert.AreEqual(model.JobDescription, "unemployed");
            Assert.AreEqual(model.JobId, 0);
            Assert.AreEqual(model.Salary, 0);
            Assert.AreEqual(model.Threshold, 0);
            Assert.AreEqual(model.EarningPoints, GameManager.DefaultUnemployedEarningPoints);
            Assert.AreEqual(model.NecessaryPoints, 0);

            

            int newJobId =
                ServiceProvider.GetJobService()
                    .AddJob(new JobModel() { JobDescription = "Des1", Threshold = 10, Salary = 2000, EarningPoints = 100, NecessaryPoints = 5000});

            Assert.IsTrue(ServiceProvider.GetUserService().GetUserPoints(player) < 5000);

            bool value = ServiceProvider.GetJobService().ChangeUserJob(newJobId, player);

            JobModel newJobModel = ServiceProvider.GetJobService().GetUserJobInfos("signalrId");

            Assert.IsFalse(value);
            Assert.AreEqual(model.JobDescription, "unemployed");
            Assert.AreEqual(model.JobId, 0);
            Assert.AreEqual(model.Salary, 0);
            Assert.AreEqual(model.Threshold, 0);
            Assert.AreEqual(model.EarningPoints, GameManager.DefaultUnemployedEarningPoints);
            Assert.AreEqual(model.NecessaryPoints, 0);

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            ServiceProvider.GetJobService().DeleteJob(newJobId);
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]

        public void AddAndDeleteJobWorks()
        {
            
            int newJobId =
                ServiceProvider.GetJobService()
                    .AddJob(new JobModel() {JobDescription = "Des1", Threshold = 10, Salary = 2000, EarningPoints = 100, NecessaryPoints = 5000});


            JobModel job = ServiceProvider.GetJobService().GetAllJobs().FirstOrDefault(x => x.JobId == newJobId);

            Assert.IsNotNull(job);
            Assert.AreEqual(job.JobDescription, "Des1");
            Assert.AreEqual(job.Threshold, 10);
            Assert.AreEqual(job.Salary, 2000);
            Assert.AreEqual(job.EarningPoints, 100);
            Assert.AreEqual(job.NecessaryPoints, 5000);

            ServiceProvider.GetJobService().DeleteJob(job);

            JobModel deletedJob = ServiceProvider.GetJobService().GetAllJobs().FirstOrDefault(x => x.JobId == newJobId);
            Assert.IsNull(deletedJob);


        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUserJobInfoNullConnectionIdThrowsException()
        {
            string t = null;
            ServiceProvider.GetJobService().GetUserJobInfos(t);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUserJobInfoNullPlayerModelThrowsException()
        {
            ServerPlayerModel t = null;
            ServiceProvider.GetJobService().GetUserJobInfos(t);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ChangeUserJobNullConnectionIdThrowsException()
        {
            string t = null;
            ServiceProvider.GetJobService().ChangeUserJob(10,t);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ChangeUserJobNullUserThrowsException()
        {
            ServerPlayerModel t = null;
            ServiceProvider.GetJobService().ChangeUserJob(10, t);
        }


    }
}
