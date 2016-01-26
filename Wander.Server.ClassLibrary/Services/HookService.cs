using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services.Interfaces;

namespace Wander.Server.ClassLibrary.Services
{
    public sealed class HookService : IHookService
    {
        private readonly ConcurrentBag<GameHook> hooks;
        private readonly List<PluginInfo> pluginsInfos = new List<PluginInfo>();
        private readonly Dictionary<string, CommandDelegate> methods = new Dictionary<string, CommandDelegate>();

        public delegate void CommandDelegate(
            IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player, CommandModel command);

        private HookService()
        {
            var h = typeof (GameHook)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof (GameHook)) && !t.IsAbstract)
                .Select(t => (GameHook) Activator.CreateInstance(t)).ToList();
            hooks = new ConcurrentBag<GameHook>(h);

            foreach (var v in hooks)
            {
                var pluginName = v.ToString();
                foreach (var attr in Attribute.GetCustomAttributes(v.GetType()))
                {
                    var info = attr as PluginInfo;
                    if (info != null)
                    {
                        var a = info;
                        pluginsInfos.Add(a);
                        pluginName = a.Name;
                    }
                    else
                    {
                        pluginName = v.ToString();
                        var pluginInfo = new PluginInfo(pluginName, "Unnamed", "Unnamed", "No version found");
                        pluginsInfos.Add(pluginInfo);
                    }
                }
                Debug.Print("Plugin loaded : " + pluginName);
            }


            foreach (GameHook g in hooks)
            {
                g.GetType()
                    .GetMethods()
                    .Where(x => x.GetCustomAttributes(typeof (ChatCommand), false).Length > 0)
                    .ForEach(m =>
                    {
                        m.GetCustomAttributes(false).ForEach(a =>
                        {
                            ChatCommand commandInfo = a as ChatCommand;
                            if (commandInfo != null)
                            {
                                methods.Add(commandInfo.Command, (CommandDelegate)Delegate.CreateDelegate(typeof (CommandDelegate),g, m));
                            }
                        });
                    });

            }


        }

        public static HookService Instance { get; } = new HookService();

        public IEnumerable<GameHook> GetHooks() => hooks;

        public List<PluginInfo> PluginsInfos()
        {
            return pluginsInfos;
        }

        public void CallHookMethod(Action<GameHook> action) => hooks.ForEach(action);

        public void CallHookCommand(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player, CommandModel command)
        {
            methods.ForEach(m =>
            {
                if (m.Key == command.Command)
                {
                    m.Value(clients, player, command);
                }
            });
        }
    }
}