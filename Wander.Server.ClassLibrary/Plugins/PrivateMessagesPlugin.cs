using System;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;
using Wander.Server.ClassLibrary.Services.Interfaces;
using static System.String;
using System.Diagnostics;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginInfo("Private messages", "Send private messages by command", "Wander", "1.0")]
    public class PrivateMessagesPlugin : GameHook
    {
        [ChatCommand("mp")]
        public void SendPrivateMessage(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            CommandModel command)
        {
                if (command.Args != null)
                {
                    if (command.Args.Length == 2)
                    {
                        var dest = command.Args[0];
                        var message = command.Args[1];
                        if (!IsNullOrEmpty(dest) && !IsNullOrEmpty(message))
                        {
                            var candidate = ServiceProvider.GetPlayerService().GetPlayerByPseudo(dest);
                            if (candidate != null)
                            {
                                var model = Helper.CreateChatMessage(player.Pseudo, candidate.UserId, message,
                                    candidate.Sex, DateTime.Now.ToShortTimeString());
                                Context.Clients.Client(candidate.SignalRId).PrivateMessageReceived(model);
                            }
                        }
                    }
                }
        }

    }
}