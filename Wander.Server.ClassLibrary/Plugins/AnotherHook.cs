using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary.Plugins
{
    public class AnotherHook : GameHook
    {
        public override void OnPlayerSendPublicMessage(Microsoft.AspNet.SignalR.Hubs.IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player, ChatMessageModel message)
        {
            clients.Caller.notify(Helper.CreateNotificationMessage("Another hook", EMessageType.success));
            base.OnPlayerSendPublicMessage(clients, player, message);
        }
    }
}
