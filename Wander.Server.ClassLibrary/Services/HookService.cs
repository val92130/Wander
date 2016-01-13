using System;
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
        private IReadOnlyCollection<GameHook> hooks;
        private static HookService _instance = new HookService();
        private HookService()
        {
            this.hooks = typeof(GameHook)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(GameHook)) && !t.IsAbstract)
                .Select(t => (GameHook)Activator.CreateInstance(t)).ToList();

            foreach (var v in hooks)
            {
                string pluginName = v.ToString();
                foreach (Attribute attr in Attribute.GetCustomAttributes(v.GetType()))
                {
                    
                    var info = attr as PluginInfo;
                    if (info != null)
                    {
                        PluginInfo a = info;
                        pluginName = a.Name;
                    }
                    else
                    {
                        pluginName = v.ToString();
                    }

                    
                }
                Debug.Print("Plugin loaded : " + pluginName);
            }

        }

        public static HookService Instance => _instance;

        public IEnumerable<GameHook> GetHooks() => hooks;

        public void CallHookMethod(Action<GameHook> action) => hooks.ForEach(action);

    }
}
