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
    [PluginInfo("AdminPlugins", "Useful commands for admins", "Wander", "1.0")]
    public class AdminPlugins : GameHook
    {
        private readonly int DefaultEventDuration = 60;
        [ChatCommand("forceRainStart", "Usage : /forceRainStart (duration in seconds)")]
        public bool ForceRainStart(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            CommandModel command)
        {
            if (ServiceProvider.GetAdminService().IsAdmin(player.UserId))
            {
                if (command.Args.Length == 1)
                {
                    if (command.Args[0] != null)
                    {
                        int duration;
                        bool success = int.TryParse(command.Args[0], out duration);
                        if (success && duration > 0)
                        {
                            clients.Caller.notify(Helper.CreateNotificationMessage(String.Format("Starting rain for {0} seconds", duration), EMessageType.success));
                            ServiceProvider.GetGameManager().ForceStartRain(duration);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (command.Args.Length > 1)
                {
                    return false;
                }
                else
                {
                    ServiceProvider.GetGameManager().ForceStartRain(DefaultEventDuration);
                    clients.Caller.notify(Helper.CreateNotificationMessage(String.Format("Starting rain for {0} seconds", DefaultEventDuration), EMessageType.success));
                }

            }
            else
            {
                clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
            }
            return true;
        }

        [ChatCommand("forceRainSop", "Usage : /forceRainStop")]
        public bool ForceRainStop(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            CommandModel command)
        {
            if (ServiceProvider.GetAdminService().IsAdmin(player.UserId))
            {
                ServiceProvider.GetGameManager().ForceStopRain();
                clients.Caller.notify(Helper.CreateNotificationMessage("Rain stopped", EMessageType.success));
            }
            else
            {
                clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
            }
            return true;
        }

        [ChatCommand("ban", "Usage : /ban 'userPseudo'")]
        public bool BanPlayer(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            CommandModel command)
        {
            if (ServiceProvider.GetAdminService().IsAdmin(player.UserId))
            {
                if (command.Args.Length == 1)
                {
                    ServerPlayerModel target = ServiceProvider.GetPlayerService().GetPlayerByPseudo(command.Args[0]);
                    if (target != null)
                    {
                        ServiceProvider.GetAdminService().BanPlayer(target.UserId);
                        clients.Client(target.SignalRId).forceDisconnect();
                        clients.Caller.notify(Helper.CreateNotificationMessage("Player banned", EMessageType.success));
                    }
                    else
                    {
                        clients.Caller.notify(Helper.CreateNotificationMessage("Player not found", EMessageType.error));
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
            }
            return true;
        }
    }
}
