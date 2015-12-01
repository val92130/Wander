using Wander.Server.ClassLibrary.Model.Forms;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public interface IUserRegistrationService
    {
        bool CheckEmailAlreadyExists(UserModel user);
        bool CheckLogin(UserModel user);
        bool CheckLoginAlreadyExists(UserModel user);
        bool CheckRegisterForm(UserModel user);
        int Connect(UserModel user);
        void Delete(ServerPlayerModel user);
        bool IsBanned(UserModel user);
        void LogOut(UserModel user);
        void LogOut(ServerPlayerModel user);
        void Register(UserModel user);
    }
}