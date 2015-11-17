﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using Microsoft.AspNet.SignalR;
using Ninject;
using Wander.Server.Model;
using Wander.Server.Services;
using Wander.Server.Model.Players;

namespace Wander.Server.Hubs
{
    public class GameHub : Hub
    {
        IUserRegistrationService _userRegistrationService;
        IPlayerService _playerService;
        IMessageService _messageService;
        IUserService _userService;
        IJobService _jobService;
        GameManager _gameManager;

        [Inject]
        IJobService JobService { get; set; }

        [Inject]
        public GameHub(IPlayerService playerService, IUserRegistrationService userRegistrationService, IMessageService messageService, IUserService userService,
            IJobService jobService, GameManager gameManager)
        {
            _userRegistrationService = userRegistrationService;
            _playerService = playerService;
            _messageService = messageService;
            _userService = userService;
            _jobService = jobService;
            _gameManager = gameManager;
        }

        public GameHub()
        {
            IJobService t = JobService;
            Debug.Print(t == null ? "Null" : "not null");
        }

        /// <summary>
        /// Connect the user to the game and into the database
        /// </summary>
        /// <param name="user"></param>
        public void Connect(UserModel user)
        {


            if (_userRegistrationService.CheckLogin(user))
            {
                List<ChatMessageModel> lastMessages = _messageService.GetMessagesLimit(5);
                Clients.Caller.LoadMessages(lastMessages);
                

                int playerId = _userRegistrationService.Connect(user);
                string idSignalR = Context.ConnectionId;
                if (playerId == -1)
                {
                    Clients.Caller.notify(Helper.CreateNotificationMessage("Connection error", EMessageType.error));
                    return;
                }

                // we check if the user isnt already connected somewhere else
                ServerPlayerModel candidate = _playerService.GetPlayer(playerId);
                if(candidate != null)
                {
                    Clients.Client(candidate.SignalRId).notify(Helper.CreateNotificationMessage("Someone else is connected from another computer", EMessageType.error));
                    Clients.Client(candidate.SignalRId).forceDisconnect();
                    _playerService.RemovePlayer(candidate.SignalRId);
                }

                Debug.Print("Client connected : " + idSignalR);

                Clients.Caller.onConnected(user.Login);
                _playerService.AddPlayer(idSignalR, playerId);

                //Notify all connected users that someone just connected
                foreach (ServerPlayerModel players in _playerService.GetAllPlayersServer())
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
            }
        }

        /// <summary>
        /// Register the User in the database if its valid
        /// </summary>
        /// <param name="user">Form fields represented by the UserModel</param>
        public void RegisterUser(UserModel user)
        {
            Debug.Print(_userRegistrationService.CheckRegisterForm(user).ToString());
            if (_userRegistrationService.CheckRegisterForm(user))
            {
                _userRegistrationService.Register(user);
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
            if (!_playerService.Exists(Context.ConnectionId))
            {
                return;
            }
            Debug.Print("Client disconnected : " + Context.ConnectionId);
            foreach (ServerPlayerModel players in _playerService.GetAllPlayersServer())
            {
                if(players.SignalRId == Context.ConnectionId) continue;
                Clients.Client(players.SignalRId).playerDisconnected(
                new { Pseudo = _userService.GetUserLogin(Context.ConnectionId) });
            }

            _userRegistrationService.LogOut(_playerService.GetPlayer(Context.ConnectionId));
            _playerService.RemovePlayer(Context.ConnectionId);
            Clients.Caller.notify(Helper.CreateNotificationMessage("See you soon !", EMessageType.info));
        }

        /// <summary>
        /// Called whenever a Client disconnects from SignalR
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            ServerPlayerModel candidate = _playerService.GetPlayer(Context.ConnectionId);
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

            ServerPlayerModel candidate = _playerService.GetPlayer(Context.ConnectionId);
            if (candidate == null)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "You have to be connected before trying to send a message ! ", EMessageType.error));
                return;
            }


            string caller = candidate.Pseudo;

            string msg = HttpUtility.HtmlEncode(message);
            List<ServerPlayerModel> ids = _playerService.GetAllPlayersServer();
            ChatMessageModel messageModel = Helper.CreateChatMessage(caller, candidate.UserId, msg,
            _userService.GetUserSex(candidate.SignalRId), DateTime.Now.ToShortTimeString());
            _messageService.LogMessage(messageModel);
            for (int i = 0; i < ids.Count;i++)
            {
                Clients.Client(ids[i].SignalRId).MessageReceived(Helper.CreateChatMessage(caller, candidate.UserId, msg, _userService.GetUserSex(candidate.SignalRId), DateTime.Now.ToShortTimeString()));
            }
        }

        /// <summary>
        /// Sends to the caller a list of every connected players
        /// </summary>
        public void GetConnectedPlayers()
        {
            Clients.Caller.showConnectedPlayers(_playerService.GetAllPlayersClient());
        }

        /// <summary>
        /// Sends to the Caller his infos
        /// </summary>
        public void GetPlayerInfo()
        {
            if (!_playerService.Exists(Context.ConnectionId))
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage(
                    "You have to be connected ! ", EMessageType.error));
                return;
            }            

            Clients.Caller.getInfos(_playerService.GetPlayerInfos(Context.ConnectionId));
        }

        /// <summary>
        /// Move the caller to the specified position
        /// </summary>
        /// <param name="position"></param>
        public void UpdatePosition(Vector2 position, EPlayerDirection direction)
        {
            if (!_playerService.Exists(Context.ConnectionId)) return;

            _playerService.MovePlayerTo(Context.ConnectionId, position, direction);
            string login = _playerService.GetPlayer(Context.ConnectionId).Pseudo;

            // Notify all the connected players that a player moved
            var players = _playerService.GetAllPlayersServer();
            object newPos = new {Pseudo = login, Position = position, Direction = direction};
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].SignalRId == Context.ConnectionId) continue;

                Clients.Client(players[i].SignalRId).playerMoved(newPos);
            }
        }

        public void GetAllPlayers()
        {
            var players = _playerService.GetAllPlayersServer();

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].SignalRId == Context.ConnectionId) continue;
                Clients.Caller.playerConnected(new {Pseudo = players[i].Pseudo, Position = players[i].Position, Direction = players[i].Direction});
            }

        }

        public void GetAllJobs()
        {
            if (!_playerService.Exists(Context.ConnectionId)) return;

            Clients.Caller.onGetJobs(_jobService.GetAllJobs().OrderBy(x => x.NecessaryPoints).ToList());
        }

        public void ApplyJob(int jobId)
        {
            if (!_playerService.Exists(Context.ConnectionId)) return;

            bool val = _jobService.ChangeUserJob(jobId, Context.ConnectionId);
            if (val)
            {
                Clients.Caller.notify(Helper.CreateNotificationMessage("Congratulations ! You got a new job ! ",
                    EMessageType.success));
            }
            else
            {
                int points = _userService.GetUserPoints(Context.ConnectionId);
                JobModel j = _jobService.GetAllJobs().FirstOrDefault(x => x.JobId == jobId);
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
            var players = _playerService.GetAllPlayersServer();

            for (int i = 0; i < players.Count; i++)
            {
                Clients.Client(players[i].SignalRId).updateTime(_gameManager.IsDay);
            }
        }
    }
}