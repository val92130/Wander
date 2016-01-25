using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services.Interfaces;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginInfo("Private messages", "Send private messages by command", "Wander", "1.0")]
    public class PrivateMessagesPlugin : GameHook
    {
        public override void OnPlayerSendCommand(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player, CommandModel command)
        {
            if (command.Command == "mp")
            {
                if (command.Args != null)
                {
                    if (command.Args.Length == 2)
                    {
                        string dest = command.Args[0];
                        string message = command.Args[1];
                        if (!string.IsNullOrEmpty(dest) && !string.IsNullOrEmpty(message))
                        {
                            var candidate = ServiceProvider.GetPlayerService().GetPlayerByPseudo(dest);
                            if (candidate != null)
                            {
                                ChatMessageModel model = Helper.CreateChatMessage(player.Pseudo, candidate.UserId, message,
                                    candidate.Sex, DateTime.Now.ToShortTimeString());
                                Context.Clients.Client(candidate.SignalRId).PrivateMessageReceived(model);
                            }
                        }
                    }
                }
            }
            base.OnPlayerSendCommand(clients, player, command);
        }
    }
}
