using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Forms;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary.Plugins
{
    public class TestHook : GameHook
    {
        public override void OnUserTyLogin(IHubCallerConnectionContext<dynamic> clients, UserModel user)
        {
            Debug.Print("User trying to login " + user.Login);
            clients.Caller.notify(Helper.CreateNotificationMessage("Heyy", Model.EMessageType.info));
            base.OnUserTyLogin(clients,user);
        }
        public override void OnPlayerSendPublicMessage(IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player, ChatMessageModel message)
        {
            clients.Caller.notify(Helper.CreateNotificationMessage("Test hook", EMessageType.success));
            base.OnPlayerSendPublicMessage(clients, player, message);
        }
    }
}
