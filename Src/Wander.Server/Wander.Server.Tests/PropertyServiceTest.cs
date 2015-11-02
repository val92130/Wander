using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander.Server.Tests
{
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
