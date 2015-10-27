using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server.Hubs
{
    public class GameHub : Hub
    {
        public void Connect(UserModel user)
        {
            Debug.Print(Context.ConnectionId);
        }

        public void RegisterUser(UserModel user)
        {
            IUserRegistrationService userRegistrationService = new DbUserRegistrationService();
            Debug.Print(userRegistrationService.CheckRegisterForm(user).ToString());
            if (userRegistrationService.CheckRegisterForm(user))
            {
                userRegistrationService.Register(user);
                Clients.Caller.sendMessage("succefully registred");
            }
            else
            {
                Clients.Caller.sendMessage("error");
            }
        }
    }
}