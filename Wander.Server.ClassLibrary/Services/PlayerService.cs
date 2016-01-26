﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Wander.Server.ClassLibrary.Hubs;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public class PlayerService : IPlayerService
    {
        private static readonly List<ServerPlayerModel> Players = new List<ServerPlayerModel>();

        /// <summary>
        ///     Add a new player to the Players list if it doesnt already exists
        /// </summary>
        /// <param name="signalRId"></param>
        /// <param name="userId"></param>
        public ServerPlayerModel AddPlayer(string signalRId, int userId)
        {
            lock (Players)
            {
                var lastPos = ServiceProvider.GetUserService().GetLastPosition(userId);

                var p = Players.FirstOrDefault(x => x.SignalRId == signalRId);
                if (p == null)
                {
                    var player = new ServerPlayerModel
                    {
                        SignalRId = signalRId,
                        UserId = userId,
                        Position = lastPos,
                        Pseudo = ServiceProvider.GetUserService().GetUserLogin(userId),
                        Direction = EPlayerDirection.Idle,
                        Sex = ServiceProvider.GetUserService().GetUserSex(userId)
                    };
                    Players.Add(player);
                    return player;
                }
                return null;
            }
        }

        /// <summary>
        ///     Checks whether a player exists
        /// </summary>
        /// <param name="signalRId"></param>
        /// <returns>Returns true if the player exists and is connected, otherwise return false</returns>
        public bool Exists(string signalRId)
        {
            return Players.FirstOrDefault(x => x.SignalRId == signalRId) != null;
        }

        public bool Exists(int userId)
        {
            return Players.FirstOrDefault(x => x.UserId == userId) != null;
        }

        /// <summary>
        ///     Remove a specified player using the SignalRId if it exists
        /// </summary>
        /// <param name="SignalRId">User's Connection Id</param>
        public bool RemovePlayer(string SignalRId)
        {
            lock (Players)
            {
                var p = GetPlayer(SignalRId);
                if (p != null)
                {
                    Players.Remove(p);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        ///     Gets a player from a specified signalRId if it exists
        /// </summary>
        /// <param name="signalRId"></param>
        /// <returns></returns>
        public ServerPlayerModel GetPlayer(string signalRId)
        {
            lock (Players)
            {
                return Players.FirstOrDefault(x => x.SignalRId == signalRId);
            }
        }

        /// <summary>
        ///     Gets a player from a specified user id if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServerPlayerModel GetPlayer(int userId)
        {
            lock (Players)
            {
                return Players.FirstOrDefault(x => x.UserId == userId);
            }
        }

        public ServerPlayerModel GetPlayerByPseudo(string pseudo)
        {
            lock (Players)
            {
                return Players.FirstOrDefault(x => x.Pseudo == pseudo);
            }
        }


        /// <summary>
        ///     Move a given user to the specified destination
        /// </summary>
        /// <param name="player"></param>
        /// <param name="to"></param>
        public bool MovePlayerTo(ServerPlayerModel player, Vector2 to, EPlayerDirection direction)
        {
            return MovePlayerTo(player.SignalRId, to, direction);
        }

        /// <summary>
        ///     Move a given user to the specified destination (using the signalR connection Id)
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="to"></param>
        public bool MovePlayerTo(string connectionId, Vector2 to, EPlayerDirection direction)
        {
            lock (Players)
            {
                var p = GetPlayer(connectionId);
                if (p == null)
                    return false;
                p.Position = to;
                p.Direction = direction;
                return true;
            }
        }


        /// <summary>
        ///     Gets a copy of all the connected players'id
        /// </summary>
        /// <returns>Returns a List of connection Id of every connected Players</returns>
        public List<string> GetAllPlayersConnectionId()
        {
            var players = new List<string>();
            lock (Players)
            {
                for (var i = 0; i < Players.Count; i++)
                {
                    players.Add(Players[i].SignalRId);
                }
            }
            return players;
        }

        /// <summary>
        ///     Gets a copy of all the connected players, this list should NOT be communicated to the client
        /// </summary>
        /// <returns>Returns a List of ServerPlayerModel of every connected Players</returns>
        public List<ServerPlayerModel> GetAllPlayersServer()
        {
            lock (Players)
            {
                var players = new List<ServerPlayerModel>();
                players = Players.ToList();
                return players;
            }
        }

        public List<ServerPlayerModel> GetAllPlayersHouse(int houseId)
        {
            lock (Players)
            {
                var players = new List<ServerPlayerModel>();
                players = Players.Where(x => x.MapId == houseId).ToList();
                return players;
            }
        }

        public bool EnterHouse(string connectionId, int houseId)
        {
            var player = Players.FirstOrDefault(x => x.SignalRId == connectionId);
            if (player == null) return false;
            player.MapId = houseId;
            player.SavedPosition = new Vector2(player.Position.X, player.Position.Y);
            player.Position = new Vector2(0, 0);
            SavePositionInDatabase(connectionId);
            return true;
        }

        public bool ExitHouse(string connectionId)
        {
            var player = Players.FirstOrDefault(x => x.SignalRId == connectionId);
            if (player == null) return false;
            player.MapId = -1;
            player.Position = player.SavedPosition;
            return true;
        }

        public bool SavePositionInDatabase(string connectionId, Vector2 newPos)
        {
            if (connectionId == null) return false;
            var player = GetPlayer(connectionId);
            if (player == null) return false;
            return ServiceProvider.GetUserService().SetLastPosition(player.UserId, newPos);
        }

        public bool SavePositionInDatabase(string connectionId)
        {
            if (connectionId == null) return false;
            var player = GetPlayer(connectionId);
            if (player == null) return false;
            return ServiceProvider.GetUserService().SetLastPosition(player.UserId, player.Position);
        }


        /// <summary>
        ///     Gets a copy of all the connected players
        /// </summary>
        /// <returns>Returns a List of ClientPlayerModel of every connected Players</returns>
        public List<ClientPlayerModel> GetAllPlayersClient()
        {
            var players = GetAllPlayersServer();
            var clientPlayers = new List<ClientPlayerModel>();

            for (var i = 0; i < players.Count; i++)
            {
                clientPlayers.Add(
                    Helper.CreateClientPlayerModel(ServiceProvider.GetUserService().GetUserLogin(players[i]),
                        ServiceProvider.GetUserService().GetUserSex(players[i]), players[i].Position));
            }
            return clientPlayers;
        }

        /// <summary>
        ///     Get a ClientPlayerModel containing every informations about the specified connectionId
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public ClientPlayerModel GetPlayerInfos(string connectionId)
        {
            if (connectionId == null)
                throw new ArgumentException("Connection id is null ! ");
            lock (Players)
            {
                var model = ServiceProvider.GetUserService().GetAllUserInfos(connectionId);
                model.Position = GetPlayer(connectionId).Position;
                model.Job = ServiceProvider.GetJobService().GetUserJobInfos(connectionId);
                model.Properties = ServiceProvider.GetPropertiesService().GetUserProperties(connectionId);
                return model;
            }
        }

        /// <summary>
        ///     Try to move a player to a specified location
        /// </summary>
        /// <param name="player"></param>
        /// <param name="to"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool TryMovePlayerTo(ServerPlayerModel player, Vector2 to, EPlayerDirection direction)
        {
            return MovePlayerTo(player, to, direction);
        }

        /// <summary>
        ///     Try to move a player to a specified location
        /// </summary>
        /// <param name="player"></param>
        /// <param name="to"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool TryMovePlayerTo(string connectionId, Vector2 to, EPlayerDirection direction)
        {
            return MovePlayerTo(connectionId, to, direction);
        }

        public bool ForcePlayerNewPosition(ServerPlayerModel player, Vector2 newPos)
        {
            if (player == null) return false;
            return ForcePlayerNewPosition(player.SignalRId, newPos);
        }

        public bool ForcePlayerNewPosition(int userId, Vector2 newPos)
        {
            lock (Players)
            {
                var p = GetPlayer(userId);
                if (p == null)
                    return false;
                p.Position = newPos;
                var context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
                context.Clients.Client(p.SignalRId).notifyNewPosition(p.Position);
                object posModel = new {p.Pseudo, Position = newPos, p.Direction};
                for (var i = 0; i < Players.Count; i++)
                {
                    if (Players[i].SignalRId == p.SignalRId) continue;

                    context.Clients.Client(Players[i].SignalRId).playerMoved(posModel);
                }
                return true;
            }
        }

        public bool ForcePlayerNewPosition(string connectionId, Vector2 newPos)
        {
            if (connectionId == null) return false;
            lock (Players)
            {
                var p = GetPlayer(connectionId);
                if (p == null)
                    return false;
                p.Position = newPos;
                var context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
                context.Clients.Client(p.SignalRId).notifyNewPosition(p.Position);
                object posModel = new {p.Pseudo, Position = newPos, p.Direction};
                for (var i = 0; i < Players.Count; i++)
                {
                    if (Players[i].SignalRId == p.SignalRId) continue;

                    context.Clients.Client(Players[i].SignalRId).playerMoved(posModel);
                }
                return true;
            }
        }
    }
}