using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wander.Server.Model;
using Wander.Server.Model.Players;

namespace Wander.Server.Services
{
    public interface IPlayerService
    {
        void AddPlayer(string ConnectionId, int UserId);
        bool Exists(string signalRId);
        void RemovePlayer(string ConnectionId);
        ServerPlayerModel GetPlayer(string ConnectionId);
        ServerPlayerModel GetPlayer(int userId);
        void MovePlayerTo(ServerPlayerModel player, Vector2 to, EPlayerDirection direction);
        void MovePlayerTo(string connectionId, Vector2 to, EPlayerDirection direction);
        List<string> GetAllPlayersConnectionId();
        List<ServerPlayerModel> GetAllPlayersServer();
        List<ClientPlayerModel> GetAllPlayersClient();
        ClientPlayerModel GetPlayerInfos(string connectionId);


    }
}