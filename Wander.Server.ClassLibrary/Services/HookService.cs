using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        public IEnumerable<GameHook> GetHooks()
        {
            return this.hooks;
        }
    }
}
