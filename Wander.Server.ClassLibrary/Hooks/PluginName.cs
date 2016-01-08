using System;

namespace Wander.Server.ClassLibrary.Hooks
{
    public class PluginName : Attribute
    {
        private string name;

        public PluginName(string name)
        {
            this.name = name;
        }
    }
}
