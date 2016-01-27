using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services.Interfaces
{
    public interface IHookService
    {
        IEnumerable<GameHook> GetHooks();
        List<PluginInfo> PluginsInfos();
        void CallHookMethod(Action<GameHook> action);
        void CallHookCommand(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player, CommandModel command);
    }
}