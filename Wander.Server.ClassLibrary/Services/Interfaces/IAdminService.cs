using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander.Server.ClassLibrary.Services.Interfaces
{
    public interface IAdminService
    {
        bool ConnectAdmin(string pseudo, string password, string connectionId);
        bool DisconnectAdmin(string connectionId);
        bool IsAdmin(int userId);
        bool IsAdminConnected(string connectionId);
        int GetPlayersTotalCount();
        int GetOnlinePlayersCount();
        bool BanPlayer(int playerId);
        bool UnBanPlayer(int playerId);
    }
}
