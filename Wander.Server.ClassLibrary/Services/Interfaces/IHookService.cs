using System;
using System.Collections.Generic;
using Wander.Server.ClassLibrary.Hooks;

namespace Wander.Server.ClassLibrary.Services.Interfaces
{
    public interface IHookService
    {
        IEnumerable<GameHook> GetHooks();
        List<PluginInfo> PluginsInfos();
        void CallHookMethod(Action<GameHook> action);
    }
}