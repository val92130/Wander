using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Forms;
using Wander.Server.ClassLibrary.Model.Job;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary.Hubs
{
    public class GameHub : Hub
    {

        /// <summary>
        /// Connect the user to the game and into the database
        /// </summary>
        /// <param name="user"></param>
        public bool Connect(UserModel user)
        {
            ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnUserTyLogin(Clients, user));
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
                if (candidate != null)
                {
                    Clients.Client(candidate.SignalRId).notify(Helper.CreateNotificationMessage("Someone else is connected from another computer", EMessageType.error));
                    Clients.Client(candidate.SignalRId).forceDisconnect();
                    ServiceProvider.GetPlayerService().RemovePlayer(candidate.SignalRId);
                }

                Debug.Print("Client connected : " + idSignalR);

                Clients.Caller.onConnected(user.Login);
                ServerPlayerModel newPlayer = ServiceProvider.GetPlayerService().AddPlayer(idSignalR, playerId);

                ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnPlayerConnect(Clients, newPlayer));

                var lastPos = ServiceProvider.GetUserService().GetLastPosition(idSignalR);
                //Notify all connected users that someone just connected
                foreach (ServerPlayerModel players in ServiceProvider.GetPlayerService().GetAllPlayersServer())
                {
                    if (players.SignalRId == Context.ConnectionId) continue;                    
                    Clients.Client(players.SignalRId).playerConnected(new { Pseudo = user.Login, Position = lastPos, Direction = EPlayerDirection.Idle });
                    if (players.MapId == -1)
                    {
                        NotifyPlayerEnterHouse(newPlayer, players);
                    }
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
        /// Connect the user to the game and into the database
        /// </summary>
        /// <param name="user"></param>
        public bool ConnectAdmin(UserModel user)
        {
            ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnAdminTryConnect(Clients, user));
            if (ServiceProvider.GetUserRegistrationService().IsBanned(user))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "Cannot login, this account has been banned", EMessageType.error));
                return false;
            }

            if (ServiceProvider.GetAdminService().ConnectAdmin(user.Login, user.Password, Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "Successfully connected", EMessageType.success));
                ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnAdminConnect(Clients, user));
                return true;
            }
            else
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "Wrong login/password", EMessageType.error));
            }
            return false;

        }

        /// <summary>
        /// Register the User in the database if its valid
        /// </summary>
        /// <param name="user">Form fields represented by the UserModel</param>
        public bool RegisterUser(UserModel user)
        {
            ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnUserTryRegister(Clients, user));
            Debug.Print(ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user).ToString());
            if (ServiceProvider.GetUserRegistrationService().CheckRegisterForm(user))
            {
                ServiceProvider.GetUserRegistrationService().Register(user);
                ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnUserTryRegister(Clients, user));
                Clients.Caller.notify(Helper.CreateNotificationMessage("Successfuly registered", EMessageType.success));
                return true;
            }
            else
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("Error while registering", EMessageType.error));
            }
            return false;
        }

        public override Task OnConnected()
        {
            ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnClientConnect(Clients));
            return base.OnConnected();
        }

        /// <summary>
        /// Called whenever a Client disconnects from SignalR
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnClientDisconnect(Clients));
            if (ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                ServiceProvider.GetAdminService().DisconnectAdmin(Context.ConnectionId);
            }
            ServerPlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            // If the disconnected client is logged in the database, we log him out
            if (candidate != null)
            {
                Disconnect();
            }

            return base.OnDisconnected(stopCalled);
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

            ServerPlayerModel disconnectingUser = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            if (disconnectingUser == null) return;
            ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnPlayerDisconnect(Clients, disconnectingUser));
            if (disconnectingUser.MapId != -1) disconnectingUser.Position = disconnectingUser.SavedPosition;

            ServiceProvider.GetUserService()
                .SetLastPosition(Context.ConnectionId,
                    disconnectingUser.Position);

            foreach (ServerPlayerModel players in ServiceProvider.GetPlayerService().GetAllPlayersServer())
            {
                if (players.SignalRId == Context.ConnectionId) continue;
                Clients.Client(players.SignalRId).playerDisconnected(
                new { Pseudo = ServiceProvider.GetUserService().GetUserLogin(Context.ConnectionId) });
                if (disconnectingUser.MapId == players.MapId)
                {
                    NotifyPlayerExitHouse(disconnectingUser, players);
                }
            }

            ServiceProvider.GetUserRegistrationService().LogOut(ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId));
            ServiceProvider.GetPlayerService().RemovePlayer(Context.ConnectionId);
            Clients.Caller.notify(Helper.CreateNotificationMessage("See you soon !", EMessageType.info));
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
            ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnPlayerSendPublicMessage(Clients, candidate, messageModel));

            CommandModel command = Helper.ParseCommand(messageModel);
            if (command != null)
            {
                ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnPlayerSendCommand(Clients, candidate, command));
                return;
            }
            ServiceProvider.GetMessageService().LogMessage(messageModel);
            for (int i = 0; i < ids.Count; i++)
            {
                Clients.Client(ids[i].SignalRId).MessageReceived(Helper.CreateChatMessage(caller, candidate.UserId, msg, ServiceProvider.GetUserService().GetUserSex(candidate.SignalRId), DateTime.Now.ToShortTimeString()));
            }

            var admins = ServiceProvider.GetAdminService().GetAllAdmins();
            for (int i = 0; i < admins.Count; i++)
            {
                Clients.Client(admins[i].ConnectionId).MessageReceived(Helper.CreateChatMessage(caller, candidate.UserId, msg, ServiceProvider.GetUserService().GetUserSex(candidate.SignalRId), DateTime.Now.ToShortTimeString()));
            }
        }

        /// <summary>
        /// Sends a private message to the specified user
        /// </summary>
        /// <param name="message"></param>
        /// <param name="destPseudo"></param>
        public void SendPrivateMessage(string message, string destPseudo)
        {
            if (String.IsNullOrWhiteSpace(message) || String.IsNullOrWhiteSpace(destPseudo))
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

            ServerPlayerModel dest =
                ServiceProvider.GetPlayerService().GetAllPlayersServer().FirstOrDefault(x => x.Pseudo == destPseudo);
            if (dest == null)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "This user doesnt exist ! ", EMessageType.error));
                return;
            }
            Clients.Client(dest.SignalRId)
                .notify(Helper.CreateNotificationMessage("You got a private message from : " + candidate.Pseudo,
                    EMessageType.info));
            string caller = candidate.Pseudo;
            ChatMessageModel model = Helper.CreateChatMessage(caller, candidate.UserId, message,
                ServiceProvider.GetUserService().GetUserSex(candidate.SignalRId), DateTime.Now.ToShortTimeString());
            ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnPlayerSendPrivateMessage(Clients, candidate, dest, model));
            Clients.Client(dest.SignalRId).PrivateMessageReceived(model);
        }

        /// <summary>
        /// Sends to the caller a list of every connected players
        /// </summary>
        public List<ClientPlayerModel> GetConnectedPlayers()
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "You have to be connected ! ", EMessageType.error));
                return null;
            }
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
        public void BuyProperty(int id)
        {
            if (ServiceProvider.GetPlayerService().Exists(Context.ConnectionId))
            {
                ServerPropertyModel m =
                    ServiceProvider.GetPropertiesService().GetProperties().FirstOrDefault(x => x.PropertyId == id);
                ServerPlayerModel buyer = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
                if (buyer == null) return;
                if (m != null)
                {
                    ServerNotificationMessage message = ServiceProvider.GetPropertiesService().BuyProperty(this.Context.ConnectionId, m);
                    if (message.MessageType == EMessageType.error)
                    {
                        Clients.Caller.notify(
                            Helper.CreateNotificationMessage("Could not buy property ! Reason : " + message.Content,
                                message.MessageType));
                    }
                    else if (message.MessageType == EMessageType.success)
                    {
                        Clients.Caller.notify(
                            Helper.CreateNotificationMessage("Success ! Enjoy your new property",
                                message.MessageType));
                        ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnPlayerBuyProperty(Clients, buyer, m));
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

            ServerPlayerModel candidate = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);

            if (candidate == null) return;

            bool success = ServiceProvider.GetPlayerService().TryMovePlayerTo(Context.ConnectionId, position, direction);
            if (!success) return;
            string login = candidate.Pseudo;

            ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnPlayerUpdatePosition(Clients, candidate, position, direction));
            // Notify all the connected players that a player moved
            var players = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            object newPos = new { Pseudo = login, Position = position, Direction = direction };
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
                Clients.Caller.playerConnected(new { Pseudo = players[i].Pseudo, Position = players[i].Position, Direction = players[i].Direction });
            }

        }

        public List<Object> GetAllPlayersMap(int mapId)
        {
            ServerPlayerModel currentPlayer = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            if (currentPlayer == null) return null;
            var players = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            List<Object> playerList = new List<Object>();
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].SignalRId == Context.ConnectionId) continue;
                if (players[i].MapId == currentPlayer.MapId)
                {
                    playerList.Add(new { Pseudo = players[i].Pseudo, Position = players[i].Position, Direction = players[i].Direction });
                }
            }
            return playerList;
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

            ServerPlayerModel seller = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            if (seller == null) return;

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

                ServiceProvider.GetHookService().CallHookMethod(hook => hook.OnPlayerSellProperty(Clients, seller, model, price));

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
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return;

            Clients.Caller.updateTime(ServiceProvider.GetGameManager().IsDay);
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

        /// <summary>
        /// Checks whether a specified is a drug dealer or not
        /// </summary>
        /// <param name="pseudo"></param>
        /// <returns></returns>
        public bool CheckIfDrugDealer(string pseudo)
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be connected to do this action", EMessageType.error));
                return false;
            }

            ServerPlayerModel candidate =
                ServiceProvider.GetPlayerService().GetAllPlayersServer().FirstOrDefault(x => x.Pseudo == pseudo);

            if (candidate == null)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("This user doesnt exist or isnt connected", EMessageType.error));
                return false;
            }

            return ServiceProvider.GetJobService().GetUserJobInfos(candidate.SignalRId).JobDescription == "Dealer";

        }

        /// <summary>
        /// Buy drug from a user
        /// </summary>
        /// <param name="dealerPseudo"></param>
        /// <returns></returns>
        public bool BuyDrug(string dealerPseudo)
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be connected to do this action", EMessageType.error));
                return false;
            }

            ServerPlayerModel candidate =
                ServiceProvider.GetPlayerService().GetAllPlayersServer().FirstOrDefault(x => x.Pseudo == dealerPseudo);

            if (candidate == null)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("This user doesnt exist or isnt connected", EMessageType.error));
                return false;
            }

            ServerNotificationMessage mess = ServiceProvider.GetJobService()
                .BuyDrugs(candidate.SignalRId, Context.ConnectionId);
            if (mess.MessageType == EMessageType.success)
            {

                return true;
            }
            else
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(mess.Content, mess.MessageType));
                return false;
            }
        }


        /// <summary>
        /// Delete a user
        /// </summary>
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

        public Vector2 GetCurrentPosition()
        {
            ServerPlayerModel player = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            if (player != null) return player.Position;
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return new Vector2();
            return ServiceProvider.GetUserService().GetLastPosition(Context.ConnectionId);
        }

        /// <summary>
        /// Gets the number of owners of a property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public int GetOwnersCount(int propertyId)
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId)) return -1;
            return ServiceProvider.GetPropertiesService().GetOwnersCount(propertyId);
        }

        /// <summary>
        /// Checks if the answer to a question is correct
        /// </summary>
        /// <param name="questId"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool CheckAnswer(int questId, bool response)
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be connected to do this action", EMessageType.error));
                return false;
            }

            if (
                ServiceProvider.GetQuestionService()
                    .CheckAnswer(new JobQuestionModel() {Answer = response, QuestionId = questId}))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("Good answer ! You win 5 points",
                    EMessageType.success));
                ServiceProvider.GetUserService()
                    .SetUserPoints(Context.ConnectionId,
                        ServiceProvider.GetUserService().GetUserPoints(Context.ConnectionId) + 5);

                return true;
            }
            else
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("Wrong answer ! You lose 5 points",
    EMessageType.error));
                ServiceProvider.GetUserService()
                    .SetUserPoints(Context.ConnectionId,
                        ServiceProvider.GetUserService().GetUserPoints(Context.ConnectionId) - 5);
                return false;
            }
        }

        private void NotifyPlayerEnterHouse(ServerPlayerModel player, ServerPlayerModel target)
        {
            if (player == null || target == null) return;
            if (player.SignalRId == target.SignalRId) return;
            Clients.Client(target.SignalRId).playerEnterMap(new { Pseudo = player.Pseudo, Position = player.Position, Direction = player.Direction });
        }

        private void NotifyPlayerExitHouse(ServerPlayerModel player, ServerPlayerModel target)
        {
            if (player == null || target == null) return;
            if (player.SignalRId == target.SignalRId) return;
            Clients.Client(target.SignalRId).playerExitMap(new { Pseudo = player.Pseudo, Position = player.Position, Direction = player.Direction });
        }

        /// <summary>
        /// Try to enter in the house with the specified propertyId
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public bool EnterHouse(int propertyId)
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be connected to do this action", EMessageType.error));
                return false;
            }
            ServerPropertyModel checkProperty = ServiceProvider.GetPropertiesService().GetProperty(propertyId);
            if (checkProperty == null) return false;
            bool value =  ServiceProvider.GetPlayerService().EnterHouse(this.Context.ConnectionId, propertyId);
            if (value)
            {
                var player = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
                if (player == null) return false;
                foreach (ServerPlayerModel p in ServiceProvider.GetPlayerService().GetAllPlayersServer())
                {
                    if (p.MapId == player.MapId)
                    {
                        NotifyPlayerEnterHouse(player, p);
                    }
                    else
                    {
                        NotifyPlayerExitHouse(player,p);
                    }
                    
                }
            }

            return value;
        }

        public bool ExitHouse()
        {
            var player = ServiceProvider.GetPlayerService().GetPlayer(Context.ConnectionId);
            if (player == null) return false;

            bool value = ServiceProvider.GetPlayerService().ExitHouse(player.SignalRId);
            if (value)
            {
                foreach (ServerPlayerModel p in ServiceProvider.GetPlayerService().GetAllPlayersServer())
                {
                    if (p.MapId == player.MapId)
                    {
                        NotifyPlayerEnterHouse(player, p);
                    }
                    else
                    {
                        NotifyPlayerExitHouse(player, p);
                    }
                }
            }
            return value;
        }

        #region admin

        /// <summary>
        /// Gets the number of connected players on the server
        /// </summary>
        /// <returns></returns>
        public int GetConnectedPlayersNumber()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return -1;
            }
            return ServiceProvider.GetPlayerService().GetAllPlayersServer().Count;
        }

        /// <summary>
        /// Gets the number of registered users
        /// </summary>
        /// <returns></returns>
        public int GetRegisteredPlayersNumber()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return -1;
            }
            return ServiceProvider.GetAdminService().GetPlayersTotalCount();
        }

        /// <summary>
        /// Gets the number of house boughts on the server
        /// </summary>
        /// <returns></returns>
        public int GetHouseBoughtsCount()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return -1;
            }
            return ServiceProvider.GetAdminService().GetBoughtsHouseCount();
        }

        /// <summary>
        /// Gets the number of message sent in the game
        /// </summary>
        /// <returns></returns>
        public int GetMessagesCount()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return -1;
            }
            return ServiceProvider.GetAdminService().GetMessagesCount();
        }

        /// <summary>
        /// Gets a list of every messages sent
        /// </summary>
        /// <returns></returns>
        public List<ChatMessageModel> GetAllMessages()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return null;
            }
            return ServiceProvider.GetAdminService().GetAllMessages();
        }

        /// <summary>
        /// Gets all the informations about every players
        /// </summary>
        /// <returns></returns>
        public List<AdminPlayerModel> GetAllPlayersAdmin()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return null;
            }
            return ServiceProvider.GetAdminService().GetAllPlayers();
        }

        public List<AdminUserModel> GetAllUsersAdmin()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return null;
            }
            return ServiceProvider.GetAdminService().GetAllUsers();
        }

        /// <summary>
        /// Ban a player
        /// </summary>
        /// <param name="userId">The player's id<</param>
        /// <returns></returns>
        public bool BanPlayer(int userId)
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return false;
            }
            var candidate = ServiceProvider.GetPlayerService().GetPlayer(userId);
            if (candidate != null)
            {
                ServiceProvider.GetPlayerService().RemovePlayer(candidate.SignalRId);
                ServiceProvider.GetUserService().SetBan(userId, true);
                Clients.Client(candidate.SignalRId).forceDisconnect();
                return true;
            }
            if(ServiceProvider.GetUserService().UserExists(userId))
            {
                ServiceProvider.GetUserService().SetBan(userId, true);
                return true;
            }
            return false;

        }

        /// <summary>
        /// Unban a player
        /// </summary>
        /// <param name="userId">The player's id</param>
        /// <returns></returns>
        public bool UnBanPlayer(int userId)
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return false;
            }

            var candidate = ServiceProvider.GetPlayerService().GetPlayer(userId);
            if (candidate != null)
            {
                ServiceProvider.GetPlayerService().RemovePlayer(candidate.SignalRId);
                ServiceProvider.GetUserService().SetBan(userId, false);
                return true;
            }
            if (ServiceProvider.GetUserService().UserExists(userId))
            {
                ServiceProvider.GetUserService().SetBan(userId, false);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Broadcast a message to every connected player
        /// </summary>
        /// <param name="message"></param>
        public void BroadcastMessageAdmin(string message)
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return;
            }

            string caller = "Admin";
            var adm = ServiceProvider.GetAdminService()
                .GetAllAdmins()
                .FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (adm == null) return;

            string msg = HttpUtility.HtmlEncode(message);
            List<ServerPlayerModel> ids = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            ChatMessageModel messageModel = Helper.CreateChatMessage(caller, adm.Id, msg,0, DateTime.Now.ToShortTimeString());
            ServiceProvider.GetMessageService().LogMessage(messageModel);
            for (int i = 0; i < ids.Count; i++)
            {
                Clients.Client(ids[i].SignalRId).notify(Helper.CreateNotificationMessage("Message from admin : " + message, EMessageType.info));
                Clients.Client(ids[i].SignalRId).MessageReceived(messageModel);
            }

            var admins = ServiceProvider.GetAdminService().GetAllAdmins();
            for (int i = 0; i < admins.Count; i++)
            {
                Clients.Client(admins[i].ConnectionId).notify(Helper.CreateNotificationMessage("Message from admin : " + message, EMessageType.info));
                Clients.Client(admins[i].ConnectionId).MessageReceived(messageModel);
            }

        }

        public void ForceStartRainAdmin(int timeInSeconds)
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return;
            }

            ServiceProvider.GetGameManager().ForceStartRain(timeInSeconds);
        }

        public void ForceStopRainAdmin()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return;
            }

            ServiceProvider.GetGameManager().ForceStopRain();
        }

        public void ForceDayAdmin(int timeInSeconds)
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return;
            }

            ServiceProvider.GetGameManager().ForceDay(timeInSeconds);
        }

        public void ForceNightAdmin(int timeInSeconds)
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return;
            }

            ServiceProvider.GetGameManager().ForceNight(timeInSeconds);
        }

        #endregion
    }
}
