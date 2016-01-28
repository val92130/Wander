using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;
using Wander.Server.ClassLibrary.Services.Interfaces;

namespace Wander.Server.ClassLibrary.Plugins
{
    public class AdminPlugins : GameHook
    {
        [ChatCommand("forceRain")]
        public bool ForceRain(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            CommandModel command)
        {
            if (ServiceProvider.GetAdminService().IsAdmin(player.UserId))
            {
                ServiceProvider.GetGameManager().ForceStartRain(60);
            }
            else
            {
                clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
            }
            return true;
        }
    }
}
