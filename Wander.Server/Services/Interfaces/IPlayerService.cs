using System.Collections.Generic;
using Wander.Server.Model;
using Wander.Server.Model.Players;

namespace Wander.Server.Services
{
    public interface IPlayerService
    {
        /// <summary>
        /// Add a new player to the Players list if it doesnt already exists
        /// </summary>
        /// <param name="signalRId"></param>
        /// <param name="userId"></param>
        void AddPlayer(string signalRId, int userId);

        /// <summary>
        /// Checks whether a player exists
        /// </summary>
        /// <param name="signalRId"></param>
        /// <returns>Returns true if the player exists and is connected, otherwise return false</returns>
        bool Exists(string signalRId);

        /// <summary>
        /// Remove a specified player using the SignalRId if it exists
        /// </summary>
        /// <param name="SignalRId">User's Connection Id</param>
        void RemovePlayer(string SignalRId);

        /// <summary>
        /// Gets a player from a specified signalRId if it exists
        /// </summary>
        /// <param name="signalRId"></param>
        /// <returns></returns>
        ServerPlayerModel GetPlayer(string signalRId);

        /// <summary>
        /// Gets a player from a specified user id if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ServerPlayerModel GetPlayer(int userId);

        /// <summary>
        /// Move a given user to the specified destination
        /// </summary>
        /// <param name="player"></param>
        /// <param name="to"></param>
        void MovePlayerTo(ServerPlayerModel player, Vector2 to, EPlayerDirection direction);

        /// <summary>
        /// Move a given user to the specified destination (using the signalR connection Id)
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="to"></param>
        void MovePlayerTo(string connectionId, Vector2 to, EPlayerDirection direction);

        /// <summary>
        /// Gets a copy of all the connected players'id
        /// </summary>
        /// <returns>Returns a List of connection Id of every connected Players</returns>
        List<string> GetAllPlayersConnectionId();

        /// <summary>
        /// Gets a copy of all the connected players, this list should NOT be communicated to the client
        /// </summary>
        /// <returns>Returns a List of ServerPlayerModel of every connected Players</returns>
        List<ServerPlayerModel> GetAllPlayersServer();

        /// <summary>
        /// Gets a copy of all the connected players
        /// </summary>
        /// <returns>Returns a List of ClientPlayerModel of every connected Players</returns>
        List<ClientPlayerModel> GetAllPlayersClient();

        /// <summary>
        /// Get a ClientPlayerModel containing every informations about the specified connectionId
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        ClientPlayerModel GetPlayerInfos(string connectionId);
    }
}