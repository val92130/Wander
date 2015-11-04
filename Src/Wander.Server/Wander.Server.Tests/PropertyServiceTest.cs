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
        public void AddAndDeletePropertyWorksCorrectly()
        {
            ServerPropertyModel model = new ServerPropertyModel();
            model.PropertyName = "TestProperty";
            model.Price = 10000;
            model.PropertyDescription = "TestDes";
            model.Threshold = 1000;

            int createdId = ServiceProvider.GetPropertiesService().AddProperty(model);
            List<ServerPropertyModel> properties = ServiceProvider.GetPropertiesService().GetProperties();

            Assert.IsTrue(properties.Count != 0);
            ServerPropertyModel candidate = properties.FirstOrDefault(x => x.PropertyId == createdId);
            Assert.IsNotNull(candidate);
            Assert.AreEqual(candidate.PropertyName, model.PropertyName);
            Assert.AreEqual(candidate.PropertyDescription, model.PropertyDescription);

            ServiceProvider.GetPropertiesService().DeleteProperty(createdId);

            Assert.IsNull(ServiceProvider.GetPropertiesService().GetProperties().FirstOrDefault(x => x.PropertyId == createdId));
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

            ServerPropertyModel model = new ServerPropertyModel();
            model.PropertyName = "TestProperty";
            model.Price = 100;
            model.PropertyDescription = "TestDes";
            model.Threshold = 1000;

            int createdId = ServiceProvider.GetPropertiesService().AddProperty(model);
            

            List<ServerPropertyModel> allProperties = ServiceProvider.GetPropertiesService().GetProperties();
            ServerPropertyModel createdProperty = allProperties.FirstOrDefault(x => x.PropertyId == createdId);


            List <ServerPropertyModel> playerProperties = ServiceProvider.GetPropertiesService().GetUserProperties(player);
            ServiceProvider.GetUserService().SetUserBankAccount(player, 2000);

            Assert.AreEqual(playerProperties.Count, 0);
            ServiceProvider.GetPropertiesService().BuyProperty("signalrId", createdProperty);
            List<ServerPropertyModel> propertyUpdated = ServiceProvider.GetPropertiesService().GetUserProperties(player);
            Assert.AreEqual(propertyUpdated.Count, 1);
            int remainedMoney = ServiceProvider.GetUserService().GetUserBankAccount(player);
            int price = createdProperty.Price;
            Assert.AreEqual((2000 -price), remainedMoney);

            ServiceProvider.GetPlayerService().RemovePlayer("signalrId");
            TestEnvironment.DeleteTestUser();
            ServiceProvider.GetPropertiesService().DeleteProperty(createdId);
        }
    }
}
