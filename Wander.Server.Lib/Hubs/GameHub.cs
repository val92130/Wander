using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Wander.Server.Model;
using Wander.Server.Services;
using Wander.Server.Model.Players;
using Wander.Server.Lib.Model;

namespace Wander.Server.Hubs
{
    public class GameHub : Hub
    {

        /// <summary>
        /// Connect the user to the game and into the database
        /// </summary>
        /// <param name="user"></param>
        public bool Connect(UserModel user)
        {

            if (ServiceProvider.GetUserRegistrationService().CheckLogin(user))
            {
                if (ServiceProvider.GetUserRegistrationService().IsBanned(user))
                {
                    Clients.Caller.notify(Helper.CreateNotificationMessage(
                        "Cannot login, this account has been banned", EMessageType.error));
                    return false;
                }
                List<ChatMessageModel> lastMessages = ServiceProvider.GetMessageService().GetMessagesLimit(5);
                Clients.Caller.LoadMessages(lastMessages);
                

                int playerId = ServiceProvider.GetUserRegistrationService().Connect(user);
                string idSignalR = Context.ConnectionId;
                if (playerId == -1)
                {
                    Clients.Caller.notify(Helper.CreateNotificationMessage("Connection error", EMessageType.error));
                    return false;
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

                //Notify all connected users that someone just connected
                foreach (ServerPlayerModel players in ServiceProvider.GetPlayerService().GetAllPlayersServer())
                {
                    if (players.SignalRId == Context.ConnectionId) continue;
                    Clients.Client(players.SignalRId).playerConnected(new { Pseudo = user.Login, Position = new Vector2(), Direction = EPlayerDirection.Idle });
                }
                Debug.Print(idSignalR);
                Clients.Caller.notify(Helper.CreateNotificationMessage("Welcome ! You are now online", EMessageType.success));
            }
            else
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("Wrong username/password", EMessageType.error));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Register the User in the database if its valid
        /// </summary>
        /// <param name="user">Form fields represented by the UserModel</param>
        public bool RegisterUser(UserModel user)
        {
            Debug.Print(ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user).ToString());
            if (ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user))
            {
                ServiceProvider.GetUserRegistrationService().Register(user);
                Clients.Caller.notify(Helper.CreateNotificationMessage("Successfuly registered", EMessageType.success));
                return true;
            }
            else
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("Error while registering", EMessageType.error));
            }
            return false;
        }

        /// <summary>
        /// Disconnect the Player related to the Caller ConnectionId
        /// </summary>
        public void Disconnect()
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId))
            {
                return;
            }
            Debug.Print("Client disconnected : " + Context.ConnectionId);
            foreach (ServerPlayerModel players in ServiceProvider.GetPlayerService().GetAllPlayersServer())
            {
                if(players.SignalRId == Context.ConnectionId) continue;
                Clients.Client(players.SignalRId).playerDisconnected(
                new { Pseudo = ServiceProvider.GetUserService().GetUserLogin(Context.ConnectionId) });
            }

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


            string caller = candidate.Pseudo;

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
        public List<ClientPlayerModel> GetConnectedPlayers()
        {
            return ServiceProvider.GetPlayerService().GetAllPlayersClient();
        }

        /// <summary>
        /// Sends to the Caller his infos
        /// </summary>
        public ClientPlayerModel GetPlayerInfo()
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "You have to be connected ! ", EMessageType.error));
                return null;
            }            

            return ServiceProvider.GetPlayerService().GetPlayerInfos(Context.ConnectionId);
        }

        /// <summary>
        /// Allow a user to buy a property
        /// </summary>
        /// <param name="id"></param>
        public void BuyProperty( int id)
        {
            if (ServiceProvider.GetPlayerService().Exists(Context.ConnectionId))
            {
                ServerPropertyModel m =
                    ServiceProvider.GetPropertiesService().GetProperties().FirstOrDefault(x => x.PropertyId == id);
                if (m != null)
                {
                    ServerNotificationMessage message = ServiceProvider.GetPropertiesService().BuyProperty(this.Context.ConnectionId, m);
                    if (message.MessageType == EMessageType.error)
                    {
                        Clients.Caller.notify(
                            Helper.CreateNotificationMessage("Could not buy property ! Reason : " + message.Content,
                                message.MessageType));
                    }
                    else if(message.MessageType == EMessageType.success)
                    {
                        Clients.Caller.notify(
                            Helper.CreateNotificationMessage("Success ! Enjoy your new property",
                                message.MessageType));
                    }

                }
                else
                {
                    Clients.Caller.notify(
                           Helper.CreateNotificationMessage("This property doesnt exist ! ", EMessageType.error));
                }
                

            }
        }

        /// <summary>
        /// Move the caller to the specified position
        /// </summary>
        /// <param name="position"></param>
        public void UpdatePosition(Vector2 position, EPlayerDirection direction)
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return;

            ServiceProvider.GetPlayerService().MovePlayerTo(Context.ConnectionId, position, direction);
            string login = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId).Pseudo;

            // Notify all the connected players that a player moved
            var players = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            object newPos = new {Pseudo = login, Position = position, Direction = direction};
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].SignalRId == Context.ConnectionId) continue;

                Clients.Client(players[i].SignalRId).playerMoved(newPos);
            }
        }

        /// <summary>
        /// Request every connected player infos
        /// </summary>
        public void GetAllPlayers()
        {
            var players = ServiceProvider.GetPlayerService().GetAllPlayersServer();

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].SignalRId == Context.ConnectionId) continue;
                Clients.Caller.playerConnected(new {Pseudo = players[i].Pseudo, Position = players[i].Position, Direction = players[i].Direction});
            }

        }

        /// <summary>
        /// Returns a list of every jobs available
        /// </summary>
        /// <returns></returns>
        public List<JobModel> GetAllJobs()
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return null;

            return ServiceProvider.GetJobService().GetAllJobs().OrderBy(x => x.NecessaryPoints).ToList();
        }

        /// <summary>
        /// Put a user's property in sell
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        public void SellProperty(int id, int price)
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return;

            ServerPropertyModel model =
                ServiceProvider.GetPropertiesService()
                    .GetUserProperties(Context.ConnectionId)
                    .FirstOrDefault(x => x.PropertyId == id);

            if (model == null)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "You dont own this property ! you cannot sell it", EMessageType.error));
                return;
            }

            if (price < 0)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "The price must be positive ! ", EMessageType.error));
                return;
            }

            ServerNotificationMessage message =
                ServiceProvider.GetPropertiesService().MakePropertyInSell(Context.ConnectionId, model, price);

            if (message.MessageType != EMessageType.success)
            {
                Clients.Caller.notify(
                    Helper.CreateNotificationMessage("Error, cannot sell this property. Reason : " + message.Content,
                        EMessageType.error));

            }
            else
            {
                Clients.Caller.notify(
                    Helper.CreateNotificationMessage("Success " + message.Content,
                        EMessageType.success));
            }
        }

        /// <summary>
        /// Try to change the user's job to the specified jobId
        /// </summary>
        /// <param name="jobId"></param>
        public void ApplyJob(int jobId)
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return;

            bool val = ServiceProvider.GetJobService().ChangeUserJob(jobId, Context.ConnectionId);
            if (val)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("Congratulations ! You got a new job ! ",
                    EMessageType.success));
            }
            else
            {
                int points = ServiceProvider.GetUserService().GetUserPoints(Context.ConnectionId);
                JobModel j = ServiceProvider.GetJobService().GetAllJobs().FirstOrDefault(x => x.JobId == jobId);
                if (j == null)
                {
                    Clients.Caller.notify(Helper.CreateNotificationMessage("This job doesnt exist ! ",
                        EMessageType.error));
                    return;
                }

                if (points < j.NecessaryPoints)
                {
                    Clients.Caller.notify(Helper.CreateNotificationMessage("You don't have enough points ! ",
                        EMessageType.error));
                    return;
                }
                Clients.Caller.notify(Helper.CreateNotificationMessage("Unknown error, please try again later ",
                        EMessageType.error));
            }
        }

        public void Update()
        {
            var players = ServiceProvider.GetPlayerService().GetAllPlayersServer();

            for (int i = 0; i < players.Count; i++)
            {
                Clients.Client(players[i].SignalRId).updateTime(ServiceProvider.GetGameManager().IsDay);
            }
        }

        /// <summary>
        /// Returns to the caller whether it's raining or not
        /// </summary>
        /// <returns></returns>
        public bool IsRaining()
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return false;

            return ServiceProvider.GetGameManager().IsRaining;
        }

        /// <summary>
        /// Gets the infos of a specified property
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServerPropertyModel GetPropertyInfo(int id)
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return null;

            ServerPropertyModel model =
                ServiceProvider.GetPropertiesService().GetProperty(id);
            return model;
        }


        public void DeleteUser()
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return;
            
            ServerPlayerModel model = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            Disconnect();
            ServiceProvider.GetUserRegistrationService().Delete(model);
            Clients.Caller.forceDisconnect();
            Clients.Caller.notify(Helper.CreateNotificationMessage("Your account was successfuly deleted",
                EMessageType.info));
        }

        public List<MoneyBag> GetMoneyBags()
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return null;

            return ServiceProvider.GetGameManager().MoneyBags;
        } 
    }
}