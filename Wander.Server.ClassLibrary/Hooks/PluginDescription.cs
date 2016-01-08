using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander.Server.ClassLibrary.Hooks
{
    public class PluginDescription : Attribute
    {
        private string description;

        public PluginDescription(string description)
        {
            this.description = description;
        }
    }
}
