using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander.Server.ClassLibrary.Services.Interfaces
{
    public interface IHookService
    {
        IEnumerable<GameHook> GetHooks();
        void CallHookMethod(Action<GameHook> action);
    }
}
