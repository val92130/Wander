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
using System.Reflection;

namespace Wander.Server.ClassLibrary.Services
{
    public sealed class HookService : IHookService
    {
        private readonly ConcurrentBag<GameHook> hooks;
        private readonly List<PluginInfo> pluginsInfos = new List<PluginInfo>();
        private readonly Dictionary<string, CommandDelegate> methods = new Dictionary<string, CommandDelegate>();

        public delegate bool CommandDelegate(
            IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player, CommandModel command);

        internal HookService()
        {
            var h = typeof (GameHook)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof (GameHook)) && !t.IsAbstract)
                .Select(t => (GameHook) Activator.CreateInstance(t)).ToList();
            hooks = new ConcurrentBag<GameHook>(h);


            foreach (GameHook g in hooks)
            {
                var pluginName = g.ToString();
                foreach (var attr in Attribute.GetCustomAttributes(g.GetType()))
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
                        var pluginInfo = new PluginInfo(pluginName, "Unnamed", "Unnamed", "No version found");
                        pluginsInfos.Add(pluginInfo);
                    }
                }
                Debug.Print("Plugin loaded : " + pluginName);

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

                                var necessaryParameters = typeof (CommandDelegate).GetMethod("Invoke").GetParameters().Select(x => x.ParameterType).ToList();
                                var currentParameters = m.GetParameters().Select( x => x.ParameterType).ToList();

                                var correct = necessaryParameters.All(currentParameters.Contains) && necessaryParameters.Count == currentParameters.Count;

                                if (correct)
                                {
                                    methods.Add(commandInfo.Command,
                                        (CommandDelegate) Delegate.CreateDelegate(typeof (CommandDelegate), g, m));
                                }
                                else
                                {
                                    throw new Exception(string.Format("Method {0} dit not match the parameters of the delegate CommandDelegate ({1})", m.Name, string.Join(",", necessaryParameters.Select(x => x.Name).ToArray())));
                                }

                            }
                        });
                    });
            }

        }


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
                    bool success = m.Value(clients, player, command);
                    if (!success)
                    {
                        ChatCommand method = (ChatCommand)m.Value.Method.GetCustomAttributes(typeof(ChatCommand), true)[0];
                        clients.Caller.notify(Helper.CreateNotificationMessage("Command error : " + method.Info,
                            EMessageType.info));
                    }
                    
                }
            });
        }
    }
}