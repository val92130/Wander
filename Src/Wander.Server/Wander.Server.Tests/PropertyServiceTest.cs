using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander.Server.Tests
{
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Wander.Server.Model;
    using Wander.Server.Model.Players;
    using Wander.Server.Services;

    [TestClass]
   public class PropertyServiceTest
    {
        [TestMethod]
        public void GetListPropertiesPlayer()
        {
            TestEnvironment.AddPropertyTest(TestEnvironment.GetTestPropertyModel());
            TestEnvironment.DeletePropertyTest();
            ServiceProvider.GetPropertiesService().GetProperties(); 
        }

        [TestMethod]
        public void GetPropertiesUserTest()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer("signalrId");
            List<ServerPropertyModel> property = ServiceProvider.GetPropertiesService().GetUserProperties(player);

            Assert.AreEqual(property.Count, 0);
            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
        }

        [TestMethod]
        public void BuyPropertyTest()
        {
            TestEnvironment.DeleteTestUser();
            UserModel user = TestEnvironment.GetTestUserModel();
            ServiceProvider.GetUserRegistrationService().Register(user);
            int id = ServiceProvider.GetUserRegistrationService().Connect(user);
            ServiceProvider.GetPlayerService().AddPlayer("signalrId", id);
            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer("signalrId");
            List<ServerPropertyModel> allProperties = ServiceProvider.GetPropertiesService().GetProperties();
            List<ServerPropertyModel> property = ServiceProvider.GetPropertiesService().GetUserProperties(player);
            ServiceProvider.GetUserService().SetUserBankAccount(player, 2000);

            Assert.AreEqual(property.Count, 0);
            ServiceProvider.GetPropertiesService().BuyProperty("signalrId", allProperties[0]);
            List<ServerPropertyModel> propertyUpdated = ServiceProvider.GetPropertiesService().GetUserProperties(player);
            Assert.AreEqual(propertyUpdated.Count, 1);
            int remainedMoney = ServiceProvider.GetUserService().GetUserBankAccount(player);
            int price = ServiceProvider.GetPropertiesService().GetProperties()[0].Price;
            Assert.AreEqual((2000 -price), remainedMoney);

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
        }
       
    }
}
