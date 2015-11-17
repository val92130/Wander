using Wander.Server.Model;

namespace Wander.Server.Services
{
    public interface IUserService
    {
        string GetUserLogin(string ConnectionId);
        string GetUserLogin(ServerPlayerModel user);
        string GetUserLoginById(int userId);
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
        ClientPlayerModel GetAllUserInfos(string ConnectionId);
        bool SetUserBankAccount(ServerPlayerModel user, int ammount);
        bool SetUserBankAccount(string ConnectionId, int ammount);
        bool SetUserPoints(ServerPlayerModel user, int ammount);
        bool SetUserPoints(string ConnectionId, int ammount);
        bool SetUserActivatedStatus(ServerPlayerModel user, bool value);
        bool SetUserActivatedStatus(string ConnectionId, bool value);
        void DeliverPay(ServerPlayerModel user);
        void DeliverPay(string ConnectionId);
        void DeliverPoints(ServerPlayerModel user);
        void DeliverPoints(string ConnectionId);
    }
}