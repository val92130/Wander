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
    public class HookService : IHookService
    {
        private IEnumerable<GameHook> hooks;
        public HookService()
        {
            this.hooks = typeof(GameHook)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(GameHook)) && !t.IsAbstract)
            .Select(t => (GameHook)Activator.CreateInstance(t));

            foreach (var v in hooks)
            {
                v.Init();

                foreach (Attribute attr in Attribute.GetCustomAttributes(v.GetType()))
                {
                    if (attr is PluginInfo)
                    {
                        PluginInfo a = (PluginInfo)attr;
                        Debug.Print("Plugin : " + a.Name);
                    }
                }
            }

        }

        public IEnumerable<GameHook> GetHooks() => hooks;

        public void CallHookMethod(Action<GameHook> action) => hooks.ForEach(action);

    }
}
