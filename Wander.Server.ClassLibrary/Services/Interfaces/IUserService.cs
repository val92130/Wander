using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public interface IUserService
    {
        bool UserExists(int userId);
        void DeliverPay(string ConnectionId);
        void DeliverPay(ServerPlayerModel user);
        void DeliverPay(int userId);
        void DeliverPoints(string ConnectionId);
        void DeliverPoints(ServerPlayerModel user);
        void DeliverPoints(int userId);
        ClientPlayerModel GetAllUserInfos(string connectionId);
        ClientPlayerModel GetAllUserInfos(int userId);
        bool GetUserActivatedStatus(string ConnectionId);
        bool GetUserActivatedStatus(ServerPlayerModel user);
        bool GetUserActivatedStatus(int userId);
        int GetUserBankAccount(string ConnectionId);
        int GetUserBankAccount(ServerPlayerModel user);
        int GetUserBankAccount(int userId);
        string GetUserEmail(string ConnectionId);
        string GetUserEmail(ServerPlayerModel user);
        string GetUserEmail(int userId);
        int GetUserJobId(string ConnectionId);
        int GetUserJobId(ServerPlayerModel user);
        int GetUserJobId(int userId);
        string GetUserLogin(string ConnectionId);
        string GetUserLogin(ServerPlayerModel user);
        string GetUserLogin(int userId);
        int GetUserPoints(string ConnectionId);
        int GetUserPoints(ServerPlayerModel user);
        int GetUserPoints(int userId);
        int GetUserSex(string ConnectionId);
        int GetUserSex(ServerPlayerModel user);
        int GetUserSex(int userId);
        Vector2 GetLastPosition(ServerPlayerModel user);
        Vector2 GetLastPosition(string ConnectionId);
        Vector2 GetLastPosition(int userId);
        bool IsBanned(string connectionId);
        bool IsBanned(ServerPlayerModel user);
        bool IsBanned(int userId);
        void PayTax(ServerPlayerModel user);
        void PayTax(int userId);
        bool SetBan(string connectionId, bool value);
        bool SetBan(ServerPlayerModel user, bool value);
        bool SetBan(int userId, bool value);
        bool SetUserActivatedStatus(string ConnectionId, bool value);
        bool SetUserActivatedStatus(ServerPlayerModel user, bool value);
        bool SetUserActivatedStatus(int userId, bool value);
        bool SetUserBankAccount(string ConnectionId, int ammount);
        bool SetUserBankAccount(ServerPlayerModel user, int ammount);
        bool SetUserBankAccount(int userId, int ammount);
        bool SetUserPoints(string ConnectionId, int ammount);
        bool SetUserPoints(ServerPlayerModel user, int ammount);
        bool SetUserPoints(int userId, int ammount);
        bool SetLastPosition(ServerPlayerModel user, Vector2 position);
        bool SetLastPosition(string connectionId, Vector2 position);
        bool SetLastPosition(int userId, Vector2 position);
        List<int> GetAllUsersId();
    }
}