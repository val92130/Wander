using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginInfo("Pay", "Deliver pay to players", "Wander", "1.0")]
    public class PayPlugin : GameHook
    {
        private DateTime _time;
        private int _intervalMinutes = 15;
        private int _alertIntervalMinutes = 2;
        protected override void Init()
        {
            _time = DateTime.Now;
            
            // Deliver the pay of every connected players
            Utilities.Timer.RepeatAfter(() =>
            {
                List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();

                for (int i = 0; i < connectedPlayers.Count; i++)
                {
                    Context.Clients.Client(connectedPlayers[i].SignalRId)
                        .notify(Helper.CreateNotificationMessage("Pay received ! ", EMessageType.success));
                    ServiceProvider.GetUserService().DeliverPay(connectedPlayers[i]);
                    ServiceProvider.GetUserService().DeliverPoints(connectedPlayers[i]);
                    _time = DateTime.Now;
                }
            }, 60 * _intervalMinutes, 60* _intervalMinutes);

            // Alert the player of the upcoming pay every 2 minutes
            Utilities.Timer.RepeatAfter(() =>
            {
                List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
                TimeSpan interval = DateTime.Now - _time;
                int remainingMinutes = _intervalMinutes - interval.Minutes;
                if (remainingMinutes == 0) return;
                for (int i = 0; i < connectedPlayers.Count; i++)
                {
                    Context.Clients.Client(connectedPlayers[i].SignalRId)
                        .notify(Helper.CreateNotificationMessage("Next pay coming in :  " + remainingMinutes + " minutes", EMessageType.info));
                }
            }, 60 * _alertIntervalMinutes, 60 * _alertIntervalMinutes);
            base.Init();
        }
    }
}
