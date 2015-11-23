using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wander.Server.Tests
{
    [TestClass]
    public class MessageServiceTests
    {
        [TestCleanup()]
        public void Cleanup()
        {
            TestEnvironment.DeleteTestUser();
        }

        [TestMethod]
        public void GetLastTwoMessagesFromDb()
        {
            // TO DO
        }

    }
}
