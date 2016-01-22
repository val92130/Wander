﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Services.Interfaces;

namespace Wander.Server.ClassLibrary.Services
{
    public sealed class HookService : IHookService
    {
        private ConcurrentBag<GameHook> hooks;
        private static HookService _instance = new HookService();
        List<PluginInfo> pluginsInfos = new List<PluginInfo>();
        private HookService()
        {
            var h  = typeof(GameHook)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(GameHook)) && !t.IsAbstract)
                .Select(t => (GameHook)Activator.CreateInstance(t)).ToList();
            this.hooks = new ConcurrentBag<GameHook>(h);

            foreach (var v in hooks)
            {
                string pluginName = v.ToString();
                foreach (Attribute attr in Attribute.GetCustomAttributes(v.GetType()))
                {
                    
                    var info = attr as PluginInfo;
                    if (info != null)
                    {
                        PluginInfo a = info;
                        pluginsInfos.Add(a);
                        pluginName = a.Name;
                    }
                    else
                    {
                        pluginName = v.ToString();
                        PluginInfo pluginInfo = new PluginInfo(pluginName, "Unnamed", "Unnamed", "No version found");
                        pluginsInfos.Add(pluginInfo);
                    }

                    
                }
                Debug.Print("Plugin loaded : " + pluginName);
            }

        }

        public static HookService Instance => _instance;

        public IEnumerable<GameHook> GetHooks() => hooks;
        public List<PluginInfo> PluginsInfos()
        {
            return pluginsInfos;
        }

        public void CallHookMethod(Action<GameHook> action) => hooks.ForEach(action);

    }
}
