using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public interface IUserRegistrationService
    {
        bool CheckRegisterForm(UserModel user);
        bool CheckLogin(UserModel user);
        bool CheckLoginAlreadyExists(UserModel user);
        bool CheckEmailAlreadyExists(UserModel user);
        int Connect(UserModel user);
        void Register(UserModel user);
        void LogOut(UserModel user);
        void LogOut(PlayerModel user);
    }
}