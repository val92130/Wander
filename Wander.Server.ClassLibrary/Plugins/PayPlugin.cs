using System;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Services;
using Wander.Server.ClassLibrary.Utilities;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginInfo("Pay", "Deliver pay to players", "Wander", "1.0")]
    public class PayPlugin : GameHook
    {
        private readonly int _alertIntervalMinutes = 2;
        private readonly int _intervalMinutes = 15;
        private DateTime _time;

        protected override void Init()
        {
            _time = DateTime.Now;

            // Deliver the pay of every connected players
            Timer.RepeatAfter(() =>
            {
                var connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();

                for (var i = 0; i < connectedPlayers.Count; i++)
                {
                    Context.Clients.Client(connectedPlayers[i].SignalRId)
                        .notify(Helper.CreateNotificationMessage("Pay received ! ", EMessageType.success));
                    ServiceProvider.GetUserService().DeliverPay(connectedPlayers[i]);
                    ServiceProvider.GetUserService().DeliverPoints(connectedPlayers[i]);
                    _time = DateTime.Now;
                }
            }, 60*_intervalMinutes, 60*_intervalMinutes);

            // Alert the player of the upcoming pay every 2 minutes
            Timer.RepeatAfter(() =>
            {
                var connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
                var interval = DateTime.Now - _time;
                var remainingMinutes = _intervalMinutes - interval.Minutes;
                if (remainingMinutes == 0) return;
                for (var i = 0; i < connectedPlayers.Count; i++)
                {
                    Context.Clients.Client(connectedPlayers[i].SignalRId)
                        .notify(
                            Helper.CreateNotificationMessage("Next pay coming in :  " + remainingMinutes + " minutes",
                                EMessageType.info));
                }
            }, 60*_alertIntervalMinutes, 60*_alertIntervalMinutes);
            base.Init();
        }

        public override void OnTick()
        {
            base.OnTick();
        }
    }
}