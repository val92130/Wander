using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Hubs;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Forms;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services.Interfaces;

namespace Wander.Server.ClassLibrary
{
    public abstract class GameHook
    {
        protected GameHook()
        {
            Init();
        }

        public IHubContext Context => GlobalHost.ConnectionManager.GetHubContext<GameHub>();

        protected virtual void Init()
        {
            Debug.Print("Init works");
        }

        public virtual void OnTick()
        {
        }

        public void OnPlayerUpdatePosition(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            Vector2 position, EPlayerDirection direction)
        {
        }

        public virtual void OnKeyDown(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            KeyModel key) {}

        public virtual void OnKeyUp(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            KeyModel key) {}

        public virtual void OnPlayerEnterHouse(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            int houseId)
            => Debug.Print("Hook on player enter house works");

        public virtual void OnPlayerExitHouse(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player)
            => Debug.Print("Hook on player exit house works");

        #region User management

        public virtual void OnUserTyLogin(IHubCallerConnectionContext<IClient> clients, UserModel user)
            => Debug.Print("Hook on player try login works");

        public virtual void OnUserTryRegister(IHubCallerConnectionContext<IClient> clients, UserModel user)
            => Debug.Print("Hook on player try register works");

        public virtual void OnUserRegister(IHubCallerConnectionContext<IClient> clients, UserModel user)
            => Debug.Print("Hook on player register works");

        public virtual void OnPlayerConnect(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player)
            => Debug.Print("Hook on player connect works");

        public virtual void OnClientDisconnect(IHubCallerConnectionContext<IClient> clients)
            => Debug.Print("Hook on client disconnect works");

        public virtual void OnClientConnect(IHubCallerConnectionContext<IClient> clients)
            => Debug.Print("Hook on client connect works");

        public virtual void OnPlayerDisconnect(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player)
            => Debug.Print("Hook on player disconnect works");

        public virtual void OnAdminTryConnect(IHubCallerConnectionContext<IClient> clients, UserModel user)
            => Debug.Print("Hook on admin try connect works");

        public virtual void OnAdminConnect(IHubCallerConnectionContext<IClient> clients, UserModel user)
            => Debug.Print("Hook on admin connect works");

        #endregion

        #region Messaging

        public virtual void OnPlayerSendPublicMessage(IHubCallerConnectionContext<IClient> clients,
            ServerPlayerModel player, ChatMessageModel message)
            => Debug.Print("Hook on player send public message works");

        public virtual void OnPlayerSendPrivateMessage(IHubCallerConnectionContext<IClient> clients,
            ServerPlayerModel sender, ServerPlayerModel target, ChatMessageModel message)
            => Debug.Print("Hook on player send private message works");

        public virtual void OnPlayerSendCommand(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            CommandModel command) => Debug.Print("Hook on player send command works");

        #endregion

        #region Properties

        public virtual void OnPlayerBuyProperty(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            ServerPropertyModel property) => Debug.Print("Hook on player buy property works");

        public virtual void OnPlayerSellProperty(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player,
            ServerPropertyModel property, int price) => Debug.Print("Hook on player sell property works");

        #endregion

        public virtual void OnRainStart() => Debug.Print("Hook on rain start works");
        public virtual void OnRainStop() => Debug.Print("Hook on rain stop works");
    }
}