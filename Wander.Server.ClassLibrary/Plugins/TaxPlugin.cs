using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginInfo("Tax", "Simulate tax payments for the players", "Wander", "1.0")]
    public class TaxPlugin : GameHook
    {
        private int _interval = 20;
        protected override void Init()
        {
            
            Utilities.Timer.Repeat(() =>
            {
                var players = ServiceProvider.GetPlayerService().GetAllPlayersServer();
                foreach (ServerPlayerModel player in players)
                {
                    Context.Clients.Client(player.SignalRId)
                    .notify(Helper.CreateNotificationMessage("You have to pay your TAX ! ", EMessageType.info));
                    ServiceProvider.GetUserService().PayTax(player);
                }
            }, 60 * _interval);
            base.Init();
        }
    }
}
