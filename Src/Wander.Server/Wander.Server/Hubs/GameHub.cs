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
                    Clients.Caller.sendMessage(new ClientMessageModel() { Content = "Connection error", MessageType = EMessageType.Error.ToString() });
                    return;
                }
               Debug.Print("Client connected : " + idSignalR);

                Clients.Caller.onConnected();
               ServiceProvider.GetPlayerService().AddPlayer(idSignalR, playerId);
               Debug.Print(idSignalR);
               Clients.Caller.sendMessage(new ClientMessageModel() {Content = "you are Online", MessageType = EMessageType.Success.ToString()});
            }
            else
            {
                Clients.Caller.sendMessage(new ClientMessageModel() { Content = "Connection error, wrong login/password", MessageType = EMessageType.Error.ToString() });
            }
        }

        public void LogOut(UserModel user)
        {
                ServiceProvider.GetUserRegistrationService().LogOut(user);
            Clients.Caller.sendMessage(new ClientMessageModel() { Content = "See you soon", MessageType = EMessageType.Information.ToString() });
        }

        public void RegisterUser(UserModel user)
        {
            Debug.Print(ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user).ToString());
            if (ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user))
            {
                ServiceProvider.GetUserRegistrationService().Register(user);
                Clients.Caller.sendMessage(new ClientMessageModel() { Content = "Successfuly registered", MessageType = EMessageType.Success.ToString() });
                Clients.Caller.onRegistered();
            }
            else
            {
                Clients.Caller.sendMessage("error");
            }
        }

        public void Disconnect()
        {
            Debug.Print("Client disconnected : " + Context.ConnectionId);
            ServiceProvider.GetPlayerService().RemovePlayer(Context.ConnectionId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Disconnect();
            return base.OnDisconnected(stopCalled);
        }
    }
}