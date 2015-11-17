using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Ninject;
using Owin;
using Wander.Server.Hubs;
using Wander.Server.Model;
using Wander.Server.Services;
using System.Reflection;

[assembly: OwinStartup(typeof(Wander.Server.Startup))]

namespace Wander.Server
{
    public class Startup
    {
        public Startup()
        {
            //GlobalHost.DependencyResolver.Resolve<GameManager>().Start();            
        }
        public void Configuration(IAppBuilder app)
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);

            app.MapSignalR();


        }



        public static void RegisterServices(IKernel kernel)
        {
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new HubActivator(kernel));
            kernel.Bind<IUserService>().To<UserService>().InSingletonScope();



            kernel.Bind<IJobService>().To<JobService>().InSingletonScope();

            kernel.Bind<IPropertyService>().To<PropertyService>().InSingletonScope();
            kernel.Bind<IMessageService>().To<MessageService>().InSingletonScope();
            kernel.Bind<IUserRegistrationService>().To<UserRegistrationServiceDb>().InSingletonScope();
            kernel.Bind<IPlayerService>().To<PlayerService>().InSingletonScope();
            kernel.Bind<GameManager>().To<GameManager>().InSingletonScope();

        }
    }
}
