using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public interface IPlayerService
    {
        void AddPlayer(string signalRId, int userId);
        bool Exists(string signalRId);
        List<ClientPlayerModel> GetAllPlayersClient();
        List<string> GetAllPlayersConnectionId();
        List<ServerPlayerModel> GetAllPlayersServer();
        List<ServerPlayerModel> GetAllPlayersHouse(int houseId);
        ServerPlayerModel GetPlayer(string signalRId);
        ServerPlayerModel GetPlayer(int userId);
        ClientPlayerModel GetPlayerInfos(string connectionId);
        bool MovePlayerTo(string connectionId, Vector2 to, EPlayerDirection direction);
        bool MovePlayerTo(ServerPlayerModel player, Vector2 to, EPlayerDirection direction);
        bool RemovePlayer(string SignalRId);
        bool TryMovePlayerTo(string connectionId, Vector2 to, EPlayerDirection direction);
        bool TryMovePlayerTo(ServerPlayerModel player, Vector2 to, EPlayerDirection direction);
        bool EnterHouse(string connectionId, int houseId);
    }
}