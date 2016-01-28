using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services.Interfaces;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginInfo("GiveMoney", "Allow players to give money to each others", "Wander", "1.0")]
    public class GiveMoneyPlugin : GameHook
    {
        [ChatCommand("give","Use like this : /give 'Destinatory', 'ammount' ")]
        public bool GiveMoney(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player, CommandModel command)
        {
            if (!string.IsNullOrEmpty(command.Command) && command.Args.Length == 2)
            {
                ServerPlayerModel dest = ServiceProvider.GetPlayerService().GetPlayerByPseudo(command.Args[0]);
                int ammount;
                bool isNumber = int.TryParse(command.Args[1], out ammount);
                if (dest != null && isNumber)
                {
                    if (ammount > 0)
                    {
                        int availableMoney = ServiceProvider.GetUserService().GetUserBankAccount(player.UserId);
                        if (availableMoney >= ammount)
                        {
                            ServiceProvider.GetUserService().SetUserBankAccount(player.UserId, availableMoney - ammount);
                            int destMoney = ServiceProvider.GetUserService().GetUserBankAccount(dest.UserId);
                            ServiceProvider.GetUserService().SetUserBankAccount(dest.UserId, destMoney + ammount);

                            Context.Clients.Client(dest.SignalRId)
                                .notify(
                                    Helper.CreateNotificationMessage(
                                        String.Format("Received {0}$ from {1}", ammount, player.Pseudo),
                                        EMessageType.success));

                            Context.Clients.Client(player.SignalRId)
                                .notify(
                                    Helper.CreateNotificationMessage(
                                        String.Format("Sent {0} to {1}", ammount, dest.Pseudo),
                                        EMessageType.success));
                        }
                        else
                        {
                            Context.Clients.Client(player.SignalRId)
                                .notify(
                                    Helper.CreateNotificationMessage("You don't have enought money",EMessageType.error));
                        }
                    }
                    else
                    {
                        clients.Caller.notify(Helper.CreateNotificationMessage("The ammount must be superior to 0",
                            EMessageType.error));
                    }
                }
                else
                {
                    clients.Caller.notify(
                        Helper.CreateNotificationMessage((dest == null ? "Cannot find this player" : "Error"),
                            EMessageType.error));
                }
                
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
