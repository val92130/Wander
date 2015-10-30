using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Wander.Server.Startup))]

namespace Wander.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
