using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
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
                List<ChatMessageModel> lastMessages = ServiceProvider.GetMessageService().GetMessagesLimit(5);
                Clients.Caller.LoadMessages(lastMessages);
                


                int playerId = ServiceProvider.GetUserRegistrationService().Connect(user);
                string idSignalR = Context.ConnectionId;
                if (playerId == -1)
                {
                    Clients.Caller.notify(Helper.CreateNotificationMessage("Connection error", EMessageType.error));
                    return;
                }

                // we check if the user isnt already connected somewhere else
                ServerPlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(playerId);
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
            ServerPlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
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
            ServerPlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
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

            if (message.Length >= 95)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "Message too long ! Your message length should be < 95 ", EMessageType.error));
                return;
            }

            ServerPlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            if (candidate == null)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "You have to be connected before trying to send a message ! ", EMessageType.error));
                return;
            }


            string caller = ServiceProvider.GetUserService().GetUserLogin(candidate.SignalRId);

            string msg = HttpUtility.HtmlEncode(message);
            List<ServerPlayerModel> ids = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            ChatMessageModel messageModel = Helper.CreateChatMessage(caller, candidate.UserId, msg,
            ServiceProvider.GetUserService().GetUserSex(candidate.SignalRId), DateTime.Now.ToShortTimeString());
            ServiceProvider.GetMessageService().LogMessage(messageModel);
            for (int i = 0; i < ids.Count;i++)
            {
                Clients.Client(ids[i].SignalRId).MessageReceived(Helper.CreateChatMessage(caller, candidate.UserId, msg, ServiceProvider.GetUserService().GetUserSex(candidate.SignalRId), DateTime.Now.ToShortTimeString()));
            }
        }

        /// <summary>
        /// Sends to the caller a list of every connected players
        /// </summary>
        public void GetConnectedPlayers()
        {
            Clients.Caller.showConnectedPlayers(ServiceProvider.GetPlayerService().GetAllPlayersClient());
        }

        /// <summary>
        /// Sends to the Caller his infos
        /// </summary>
        public void GetPlayerInfo()
        {
            ServerPlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            if (candidate == null)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "You have to be connected ! ", EMessageType.error));
                return;
            }            

            Clients.Caller.getInfos(ServiceProvider.GetPlayerService().GetPlayerInfos(Context.ConnectionId));
        }

        /// <summary>
        /// Move the caller to the specified position
        /// </summary>
        /// <param name="position"></param>
        public void MoveTo(Vector2 position)
        {
            ServerPlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            // If the caller is not logged into the game, we dont do anything
            if (candidate == null)
            {
                return;
            }

            ServiceProvider.GetPlayerService().MovePlayerTo(Context.ConnectionId, position);
        }
    }
}