using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;
using Microsoft.AspNet.SignalR;
using Wander.Server.Hubs;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server
{
    public class GameManager
    {
        Timer payTime = new Timer();
        private static int count = 0;
        IHubContext context;
        public GameManager()
        {
            context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            payTime.Interval = 1000 * 60 * 15; // Every 15 minutes
            payTime.Elapsed += Elapsed;
            payTime.Start();
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.Print("Delivering pay ! " + count++);
            List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();

            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                context.Clients.Client(connectedPlayers[i].SignalRId)
                    .notify(Helper.CreateNotificationMessage("Pay received ! ", EMessageType.success));
                ServiceProvider.GetUserService().DeliverPay(connectedPlayers[i]);
                ServiceProvider.GetUserService().DeliverPoints(connectedPlayers[i]);
            }
        }
    }
}