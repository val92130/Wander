using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server.Hubs
{
    public class GameHub : Hub
    {
        
        public void Connect(UserModel user)
        {
            if (ServiceProvider.GetUserRegistrationService().CheckLogin(user))
            {
                ServiceProvider.GetUserRegistrationService().Connect(user);
                Clients.Caller.sendMessage("you are Online");
            }
            else
            {
                Clients.Caller.sendMessage("connexion error");
            }
        }

        public void LogOut(UserModel user)
        {
            
                ServiceProvider.GetUserRegistrationService().LogOut(user);
                Clients.Caller.sendMessage("see you soon");
            
           
        }

        public void RegisterUser(UserModel user)
        {
            Debug.Print(ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user).ToString());
            if (ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user))
            {
                ServiceProvider.GetUserRegistrationService().Register(user);
                Clients.Caller.sendMessage("succefully registred");
            }
            else
            {
                Clients.Caller.sendMessage("error");
            }
        }
    }
}