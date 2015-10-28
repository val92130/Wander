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

        /// <summary>
        /// Connect the user to the game and into the database
        /// </summary>
        /// <param name="user"></param>
        public void Connect(UserModel user)
        {
            if (ServiceProvider.GetUserRegistrationService().CheckLogin(user))
            {
                int playerId = ServiceProvider.GetUserRegistrationService().Connect(user);
                string idSignalR = Context.ConnectionId;
                if (playerId == -1)
                {
                    Clients.Caller.notify(Helper.CreateNotificationMessage("Connection error", EMessageType.error));
                    return;
                }

                // we check if the user isnt already connected somewhere else
                PlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(playerId);
                if(candidate != null)
                {
                    Clients.Client(candidate.SignalRId).notify(Helper.CreateNotificationMessage("Someone else is connected from another computer", EMessageType.error));
                    Clients.Client(candidate.SignalRId).forceDisconnect();
                    ServiceProvider.GetPlayerService().RemovePlayer(candidate.SignalRId);
                }

                Debug.Print("Client connected : " + idSignalR);

                Clients.Caller.onConnected(user.Login);
                ServiceProvider.GetPlayerService().AddPlayer(idSignalR, playerId);
                Debug.Print(idSignalR);
                Clients.Caller.notify(Helper.CreateNotificationMessage("Welcome ! You are now online", EMessageType.success));
            }
            else
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("Wrong username/password", EMessageType.error));
            }
        }

        /// <summary>
        /// Register the User in the database if its valid
        /// </summary>
        /// <param name="user">Form fields represented by the UserModel</param>
        public void RegisterUser(UserModel user)
        {
            Debug.Print(ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user).ToString());
            if (ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user))
            {
                ServiceProvider.GetUserRegistrationService().Register(user);
                Clients.Caller.notify(Helper.CreateNotificationMessage("Successfuly registered", EMessageType.success));
                Clients.Caller.onRegistered();
            }
            else
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("Error while registering", EMessageType.error));
            }
        }

        /// <summary>
        /// Disconnect the Player related to the Caller ConnectionId
        /// </summary>
        public void Disconnect()
        {
            PlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            // If the disconnected client is logged in the database, we log him out
            if (candidate == null)
            {
                return;
            }
            Debug.Print("Client disconnected : " + Context.ConnectionId);
            ServiceProvider.GetUserRegistrationService().LogOut(ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId));
            ServiceProvider.GetPlayerService().RemovePlayer(Context.ConnectionId);
            Clients.Caller.notify(Helper.CreateNotificationMessage("See you soon !", EMessageType.info));
        }

        /// <summary>
        /// Called whenever a Client disconnects from SignalR
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Broadcast the Caller's message to all the Clients connected to the game
        /// </summary>
        /// <param name="message"></param>
        public void SendPublicMessage(string message)
        {
            if (String.IsNullOrWhiteSpace(message))
                return;

            PlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            if (candidate == null)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "You have to be connected before trying to send a message ! ", EMessageType.error));
                return;
            }


            string caller = ServiceProvider.GetUserService().GetUserLogin(candidate.SignalRId);

            string msg = HttpUtility.HtmlEncode(message);
            List<PlayerModel> ids = ServiceProvider.GetPlayerService().GetAllPlayers();
            for (int i = 0; i < ids.Count;i++)
            {
                //Clients.Client(ids[i].SignalRId).MessageReceived(msg, caller);
                Clients.Client(ids[i].SignalRId).MessageReceived(Helper.CreateChatMessage(caller, msg, ServiceProvider.GetUserService().GetUserSex(candidate.SignalRId), DateTime.Now.ToShortTimeString()));
            }
        }
    }
}