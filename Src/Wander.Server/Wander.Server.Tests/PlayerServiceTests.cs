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
    }
}
