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
                ServiceProvider.GetPlayerService().AddPlayer(idSignalR, playerId);
                var lastPos = ServiceProvider.GetUserService().GetLastPosition(idSignalR);

                //Notify all connected users that someone just connected
                foreach (ServerPlayerModel players in ServiceProvider.GetPlayerService().GetAllPlayersServer())
                {
                    if (players.SignalRId == Context.ConnectionId) continue;
                    Clients.Client(players.SignalRId).playerConnected(new { Pseudo = user.Login, Position = lastPos, Direction = EPlayerDirection.Idle });
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
        /// Called whenever a Client disconnects from SignalR
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
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
            ServiceProvider.GetUserService()
                .SetLastPosition(Context.ConnectionId,
                    ServiceProvider.GetPlayerService().GetPlayerInfos(Context.ConnectionId).Position);
            foreach (ServerPlayerModel players in ServiceProvider.GetPlayerService().GetAllPlayersServer())
            {
                if (players.SignalRId == Context.ConnectionId) continue;
                Clients.Client(players.SignalRId).playerDisconnected(
                new { Pseudo = ServiceProvider.GetUserService().GetUserLogin(Context.ConnectionId) });
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
            Clients.Client(dest.SignalRId).PrivateMessageReceived(Helper.CreateChatMessage(caller, candidate.UserId, message, ServiceProvider.GetUserService().GetUserSex(candidate.SignalRId), DateTime.Now.ToShortTimeString()));
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

        public bool EnterHouse(int propertyId)
        {
            if (!ServiceProvider.GetPlayerService().Exists(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be connected to do this action", EMessageType.error));
                return false;
            }
            ServerPropertyModel checkProperty = ServiceProvider.GetPropertiesService().GetProperty(propertyId);
            if (checkProperty == null) return false;
           return ServiceProvider.GetPlayerService().EnterHouse(this.Context.ConnectionId, propertyId);
        }

        #region admin

        public int GetConnectedPlayersNumber()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return -1;
            }
            return ServiceProvider.GetPlayerService().GetAllPlayersServer().Count;
        }

        public int GetRegisteredPlayersNumber()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return -1;
            }
            return ServiceProvider.GetAdminService().GetPlayersTotalCount();
        }

        public int GetHouseBoughtsCount()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return -1;
            }
            return ServiceProvider.GetAdminService().GetBoughtsHouseCount();
        }

        public int GetMessagesCount()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return -1;
            }
            return ServiceProvider.GetAdminService().GetMessagesCount();
        }

        public List<ChatMessageModel> GetAllMessages()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return null;
            }
            return ServiceProvider.GetAdminService().GetAllMessages();
        }

        public List<AdminPlayerModel> GetAllPlayersAdmin()
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return null;
            }
            return ServiceProvider.GetAdminService().GetAllPlayers();
        }

        public void BanPlayer(int userId)
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return;
            }

            ServiceProvider.GetUserService().SetBan(new ServerPlayerModel() {UserId = userId}, true);
        }

        public void UnBanPlayer(int userId)
        {
            if (!ServiceProvider.GetAdminService().IsAdminConnected(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("You have to be an admin", EMessageType.error));
                return;
            }

            ServiceProvider.GetUserService().SetBan(new ServerPlayerModel() { UserId = userId }, false);
        }

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

        #endregion
    }
}
