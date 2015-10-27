using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
               int playerId = ServiceProvider.GetUserRegistrationService().Connect(user);
               string idSignalR = Context.ConnectionId;
                if (playerId == -1)
                {
                    Clients.Caller.sendMessage("connexion error");
                    return;
                }
               ServiceProvider.GetPlayerService().AddPlayer(idSignalR, playerId);
               Debug.Print(idSignalR);
               Clients.Caller.sendMessage("you are Online");
            }
            else
            {
                Clients.Caller.sendMessage("connexion error, wrong login/password");
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
                Clients.Caller.onRegistered();
            }
            else
            {
                Clients.Caller.sendMessage("error");
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Debug.Print("Client disconnected : " + Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}