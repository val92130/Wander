using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Wander.Server.Startup))]
namespace Wander.Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
