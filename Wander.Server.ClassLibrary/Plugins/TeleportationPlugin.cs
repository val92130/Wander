﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginName("Teleportation")]
    [PluginDescription("Teleports a player to another player")]
    public class TeleportationPlugin : GameHook
    {
        public override void OnPlayerSendCommand(IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player, CommandModel command)
        {
            if (command == null || player == null || clients == null) return;

            if (command.Command == "tp")
            {
                if (command.Args.Length != 0)
                {
                    string toPlayer = command.Args[0];
                    ServerPlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayerByPseudo(toPlayer);

                    if (candidate != null)
                    {

                        if (candidate.Pseudo == player.Pseudo)
                        {
                            clients.Caller.notify(
                                Helper.CreateNotificationMessage("Cannot teleport to yourself",
                                    EMessageType.info));
                        }
                        else
                        {
                            ServiceProvider.GetPlayerService().ForcePlayerNewPosition(player, candidate.Position);
                            clients.Caller.notify(
                                Helper.CreateNotificationMessage("Teleporting you to player : " + candidate.Pseudo,
                                    EMessageType.success));
                        }
                    }
                    else
                    {
                        clients.Caller.notify(
                            Helper.CreateNotificationMessage("Can't find player : " + candidate.Pseudo,
                                EMessageType.error));
                    }
                }
            }
            base.OnPlayerSendCommand(clients, player, command);
        }
    }
}
