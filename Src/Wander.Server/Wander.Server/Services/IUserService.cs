using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public interface IUserService
    {
        string GetUserLogin(string ConnectionId);
        string GetUserLogin(PlayerModel user);
        string GetUserEmail(PlayerModel user);
        string GetUserEmail(string ConnectionId);
        int GetUserSex(PlayerModel user);
        int GetUserSex(string ConnectionId);
        int GetUserBankAccount(PlayerModel user);
        int GetUserBankAccount(string ConnectionId);
        int GetUserPoints(PlayerModel user);
        int GetUserPoints(string ConnectionId);
        
        bool GetUserActivatedStatus(PlayerModel user);
        bool GetUserActivatedStatus(string ConnectionId);
        int GetUserJobId(PlayerModel user);
        int GetUserJobId(string ConnectionId);
    }
}