using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public interface IUserService
    {
        void DeliverPay(string ConnectionId);
        void DeliverPay(ServerPlayerModel user);
        void DeliverPoints(string ConnectionId);
        void DeliverPoints(ServerPlayerModel user);
        ClientPlayerModel GetAllUserInfos(string ConnectionId);
        bool GetUserActivatedStatus(string ConnectionId);
        bool GetUserActivatedStatus(ServerPlayerModel user);
        int GetUserBankAccount(string ConnectionId);
        int GetUserBankAccount(ServerPlayerModel user);
        string GetUserEmail(string ConnectionId);
        string GetUserEmail(ServerPlayerModel user);
        int GetUserJobId(string ConnectionId);
        int GetUserJobId(ServerPlayerModel user);
        string GetUserLogin(string ConnectionId);
        string GetUserLogin(ServerPlayerModel user);
        string GetUserLoginById(int userId);
        int GetUserPoints(string ConnectionId);
        int GetUserPoints(ServerPlayerModel user);
        int GetUserSex(string ConnectionId);
        int GetUserSex(ServerPlayerModel user);
        Vector2 GetLastPosition(ServerPlayerModel user);
        Vector2 GetLastPosition(string ConnectionId);
        Vector2 GetLastPosition(int userId);
        bool IsBanned(string connectionId);
        bool IsBanned(ServerPlayerModel user);
        void PayTax(ServerPlayerModel user);
        bool SetBan(string connectionId, bool value);
        bool SetBan(ServerPlayerModel user, bool value);
        bool SetUserActivatedStatus(string ConnectionId, bool value);
        bool SetUserActivatedStatus(ServerPlayerModel user, bool value);
        bool SetUserBankAccount(string ConnectionId, int ammount);
        bool SetUserBankAccount(ServerPlayerModel user, int ammount);
        bool SetUserPoints(string ConnectionId, int ammount);
        bool SetUserPoints(ServerPlayerModel user, int ammount);
        bool SetLastPosition(ServerPlayerModel user, Vector2 position);
        bool SetLastPosition(string ConnectionId, Vector2 position);
    }
}