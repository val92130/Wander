﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
