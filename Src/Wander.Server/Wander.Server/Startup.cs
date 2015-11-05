using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Owin;
using Owin;
using Wander.Server.Model;
using Wander.Server.Services;

[assembly: OwinStartup(typeof(Wander.Server.Startup))]

namespace Wander.Server
{
    public class Startup
    {
        Timer payTime = new Timer();
        private static int count = 0;
        public Startup()
        {
            payTime.Interval = 3600000; // every hour
            payTime.Elapsed += Elapsed;
            payTime.Start();
        }
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.Print("Delivering pay ! "+ count++);
            List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();

            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                ServiceProvider.GetUserService().DeliverPay(connectedPlayers[i]);
                ServiceProvider.GetUserService().DeliverPoints(connectedPlayers[i]);
            }
        }
    }
}
