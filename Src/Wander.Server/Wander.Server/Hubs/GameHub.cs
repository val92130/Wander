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
                    Clients.Caller.notify(Helper.CreateMessage("Connection error", EMessageType.error));
                    return;
                }
                Debug.Print("Client connected : " + idSignalR);

                Clients.Caller.onConnected();
                ServiceProvider.GetPlayerService().AddPlayer(idSignalR, playerId);
                Debug.Print(idSignalR);
                Clients.Caller.notify(Helper.CreateMessage("Welcome ! You are now online", EMessageType.success));
            }
            else
            {
                Clients.Caller.notify(Helper.CreateMessage("Wrong username/password", EMessageType.error));
            }
        }


        public void RegisterUser(UserModel user)
        {
            Debug.Print(ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user).ToString());
            if (ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user))
            {
                ServiceProvider.GetUserRegistrationService().Register(user);
                Clients.Caller.notify(Helper.CreateMessage("Successfuly registered", EMessageType.success));
                Clients.Caller.onRegistered();
            }
            else
            {
                Clients.Caller.notify(Helper.CreateMessage("Error while registering", EMessageType.error));
            }
        }

        public void Disconnect()
        {
            Debug.Print("Client disconnected : " + Context.ConnectionId);
            ServiceProvider.GetUserRegistrationService().LogOut(ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId));
            ServiceProvider.GetPlayerService().RemovePlayer(Context.ConnectionId);
            Clients.Caller.notify(Helper.CreateMessage("See you soon !", EMessageType.info));
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            PlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            // If the disconnected client is logged in the database, we log him out
            if (candidate != null)
            {
                Disconnect();
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}