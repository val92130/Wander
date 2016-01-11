using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary.Plugins
{
    public class TestTimer : GameHook
    {
        public override void Init()
        {
            base.Init();
        }

        public override void OnPlayerConnect(Microsoft.AspNet.SignalR.Hubs.IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player)
        {
            Utilities.Timer.Once(() =>
            {
                clients.Caller.notify(Helper.CreateNotificationMessage("Hey, working", Model.EMessageType.info));
            }, 10);

            Utilities.Timer.Repeat(() =>
            {
                clients.Caller.notify(Helper.CreateNotificationMessage("5 second elapsed", Model.EMessageType.info));
            }, 5);

            Utilities.Timer.RepeatAfter(() =>
            {
                clients.Caller.notify(Helper.CreateNotificationMessage("20 second elapsed", Model.EMessageType.info));
            },20, 5);

            base.OnPlayerConnect(clients, player);
        }

    }
}
