using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services.Interfaces
{
    public interface IAdminService
    {
        List<AdminModel> GetAllAdmins();
        bool ConnectAdmin(string pseudo, string password, string connectionId);
        bool DisconnectAdmin(string connectionId);
        bool IsAdmin(int userId);
        bool IsAdminConnected(string connectionId);
        int GetPlayersTotalCount();
        int GetOnlinePlayersCount();
        bool BanPlayer(int playerId);
        bool UnBanPlayer(int playerId);
        int GetMessagesCount();
        List<ChatMessageModel> GetAllMessages();
        int GetBoughtsHouseCount();
        List<AdminPlayerModel> GetAllPlayers();
        AdminPlayerModel GetPlayerInfo(int userId);
        AdminPlayerModel GetPlayerInfo(string connectionId);
        bool SetDay(bool value);
        bool SetRain(bool value);
    }
}
