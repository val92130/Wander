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
        string GetUserLogin(ServerPlayerModel user);
        string GetUserEmail(ServerPlayerModel user);
        string GetUserEmail(string ConnectionId);
        int GetUserSex(ServerPlayerModel user);
        int GetUserSex(string ConnectionId);
        int GetUserBankAccount(ServerPlayerModel user);
        int GetUserBankAccount(string ConnectionId);
        int GetUserPoints(ServerPlayerModel user);
        int GetUserPoints(string ConnectionId);
        
        bool GetUserActivatedStatus(ServerPlayerModel user);
        bool GetUserActivatedStatus(string ConnectionId);
        int GetUserJobId(ServerPlayerModel user);
        int GetUserJobId(string ConnectionId);
    }
}