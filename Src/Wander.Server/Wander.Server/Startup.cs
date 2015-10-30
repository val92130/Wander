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
        Timer t = new Timer();
        private static int count = 0;
        public Startup()
        {
            t.Interval = 1000;
            t.Elapsed += Elapsed;
            t.Start();
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.Print("ok" + count++);
        }

        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
