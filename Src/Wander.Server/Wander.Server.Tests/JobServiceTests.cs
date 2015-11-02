using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server.Tests { 

    [TestClass]
    public class JobServiceTests
    {
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

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();

        }

        [TestMethod]
        public void ChangerUserJobServerModelWorks()
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

        public void AddAndDeleteJobWorks()
        {
            
            int newJobId =
                ServiceProvider.GetJobService()
                    .AddJob(new JobModel() {JobDescription = "Des1", Threshold = 10, Salary = 2000});


            JobModel job = ServiceProvider.GetJobService().GetAllJobs().FirstOrDefault(x => x.JobId == newJobId);

            Assert.IsNotNull(job);
            Assert.AreEqual(job.JobDescription, "Des1");
            Assert.AreEqual(job.Threshold, 10);
            Assert.AreEqual(job.Salary, 2000);

            ServiceProvider.GetJobService().DeleteJob(job);

            JobModel deletedJob = ServiceProvider.GetJobService().GetAllJobs().FirstOrDefault(x => x.JobId == newJobId);
            Assert.IsNull(deletedJob);


        }
    }
}
