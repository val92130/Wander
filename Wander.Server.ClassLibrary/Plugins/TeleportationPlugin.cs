using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;
using Wander.Server.ClassLibrary.Services.Interfaces;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginInfo("Teleportation", "Teleports a player to another player", "Wander", "1.0")]
    public class TeleportationPlugin : GameHook
    {
        public override void OnPlayerSendCommand(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            CommandModel command)
        {
            if (command == null || player == null || clients == null) return;
            if (command.Command == "tp")
            {
                if (command.Args.Length != 0)
                {
                    var toPlayer = command.Args[0];
                    var candidate = ServiceProvider.GetPlayerService().GetPlayerByPseudo(toPlayer);

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
                            Helper.CreateNotificationMessage("Can't find player" + player.Pseudo,
                                EMessageType.error));
                    }
                }
            }
            base.OnPlayerSendCommand(clients, player, command);
        }
    }
}