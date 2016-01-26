using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Services;
using Wander.Server.ClassLibrary.Utilities;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginInfo("Tax", "Simulate tax payments for the players", "Wander", "1.0")]
    public class TaxPlugin : GameHook
    {
        private readonly int _interval = 20;

        protected override void Init()
        {
            Timer.Repeat(() =>
            {
                var players = ServiceProvider.GetPlayerService().GetAllPlayersServer();
                foreach (var player in players)
                {
                    Context.Clients.Client(player.SignalRId)
                        .notify(Helper.CreateNotificationMessage("You have to pay your TAX ! ", EMessageType.info));
                    ServiceProvider.GetUserService().PayTax(player);
                }
            }, 60*_interval);
            base.Init();
        }
    }
}