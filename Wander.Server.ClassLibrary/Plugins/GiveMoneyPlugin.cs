using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services.Interfaces;

namespace Wander.Server.ClassLibrary.Plugins
{
    public class GiveMoneyPlugin : GameHook
    {
        public override void OnPlayerSendCommand(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player, CommandModel command)
        {
            if (!string.IsNullOrEmpty(command.Command) && command.Args.Length == 2)
            {
                // to do
            }
            base.OnPlayerSendCommand(clients, player, command);
        }
    }
}
