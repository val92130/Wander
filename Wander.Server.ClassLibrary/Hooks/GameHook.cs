using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Forms;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary
{
    public abstract class GameHook
    {
        #region User management
        public virtual void OnUserTyLogin(IHubCallerConnectionContext<dynamic> clients, UserModel user)
        {
            Debug.Print("Hook on player try login works");
        }
        public virtual void OnUserTryRegister(IHubCallerConnectionContext<dynamic> clients, UserModel user)
        {
            Debug.Print("Hook on player try register works");
        }
        public virtual void OnUserRegister(IHubCallerConnectionContext<dynamic> clients, UserModel user)
        {
            Debug.Print("Hook on player register works");
        }

        public virtual void OnPlayerConnect(IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player)
        {
            Debug.Print("Hook on player connect works");
        }
        public virtual void OnClientDisconnect(IHubCallerConnectionContext<dynamic> clients)
        {
            Debug.Print("Hook on client disconnect works");
        }
        public virtual void OnClientConnect(IHubCallerConnectionContext<dynamic> clients)
        {
            Debug.Print("Hook on client connect works");
        }
        public virtual void OnPlayerDisconnect(IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player)
        {
            Debug.Print("Hook on player disconnect works");
        }

        public virtual void OnAdminTryConnect(IHubCallerConnectionContext<dynamic> clients, UserModel user)
        {
            Debug.Print("Hook on admin try connect works");
        }

        public virtual void OnAdminConnect(IHubCallerConnectionContext<dynamic> clients, UserModel user)
        {
            Debug.Print("Hook on admin connect works");
        }
        #endregion

        #region Messaging
        public virtual void OnPlayerSendPublicMessage(IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player, ChatMessageModel message)
        {
            Debug.Print("Hook on player send public message works");
        }
        public virtual void OnPlayerSendPrivateMessage(IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel sender,ServerPlayerModel target, ChatMessageModel message)
        {
            Debug.Print("Hook on player send private message works");
        }

        public virtual void OnPlayerSendCommand(IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player,CommandModel command)
        {
            Debug.Print("Hook on player send command works");
        }
        #endregion

        #region Properties
        public virtual void OnPlayerBuyProperty(IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player, ServerPropertyModel property)
        {
            Debug.Print("Hook on player buy property works");
        }

        public virtual void OnPlayerSellProperty(IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player, ServerPropertyModel property, int price)
        {
            Debug.Print("Hook on player sell property works");
        }
        #endregion

        public void OnPlayerUpdatePosition(IHubCallerConnectionContext<dynamic> clients, ServerPlayerModel player,
            Vector2 position, EPlayerDirection direction)
        {
            Debug.Print("Hook on player update position works");
        }
    }
}
