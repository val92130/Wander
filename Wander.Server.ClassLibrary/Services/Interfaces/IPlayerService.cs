﻿using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public interface IPlayerService
    {
        ServerPlayerModel AddPlayer(string signalRId, int userId);
        bool Exists(string signalRId);
        List<ClientPlayerModel> GetAllPlayersClient();
        List<string> GetAllPlayersConnectionId();
        List<ServerPlayerModel> GetAllPlayersServer();
        List<ServerPlayerModel> GetAllPlayersHouse(int houseId);
        ServerPlayerModel GetPlayer(string signalRId);
        ServerPlayerModel GetPlayer(int userId);
        ServerPlayerModel GetPlayerByPseudo(string pseudo);
        ClientPlayerModel GetPlayerInfos(string connectionId);
        bool MovePlayerTo(string connectionId, Vector2 to, EPlayerDirection direction);
        bool MovePlayerTo(ServerPlayerModel player, Vector2 to, EPlayerDirection direction);
        bool RemovePlayer(string SignalRId);
        bool TryMovePlayerTo(string connectionId, Vector2 to, EPlayerDirection direction);
        bool TryMovePlayerTo(ServerPlayerModel player, Vector2 to, EPlayerDirection direction);
        bool ForcePlayerNewPosition(ServerPlayerModel player, Vector2 newPos);
        bool ForcePlayerNewPosition(int userId, Vector2 newPos);
        bool ForcePlayerNewPosition(string connectionId, Vector2 newPos);
        bool EnterHouse(string connectionId, int houseId);
        bool ExitHouse(string connectionId);
        bool SavePositionInDatabase(string connectionId, Vector2 newPos);
        bool SavePositionInDatabase(string connectionId);
        bool Exists(int userId);
    }
}