using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.ClassLibrary.Model.Job;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.Tests
{
    [TestClass]
    public class QuestionsTests
    {
        [TestMethod]
        public void GetRandomQuestionWorks()
        {
            TestEnvironment.DeleteTestUser();

            var u = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(u);
            int id = ServiceProvider.GetUserRegistrationService().Connect(u);

            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServiceProvider.GetUserService().SetUserPoints("signalrId", 5000);
            ServiceProvider.GetJobService().ChangeUserJob(1, "signalrId");

            Assert.AreEqual(ServiceProvider.GetJobService().GetUserJobInfos("signalrId").JobId, 1);

            JobQuestionModel m = ServiceProvider.GetQuestionService().GetRandomQuestion("signalrId");
            Console.WriteLine(m.Question + " : " + m.Answer);
            Assert.IsNotNull(m);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }
    }
}
