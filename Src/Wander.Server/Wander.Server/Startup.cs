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
        GameManager game;
        public Startup()
        {
            ServiceProvider.GetGameManager().Start();            
        }
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
