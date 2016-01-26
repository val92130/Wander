using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander.Server.ClassLibrary.Hooks
{
    public class ChatCommand : System.Attribute
    {
        private string _command;
        public ChatCommand(string command)
        {
            this._command = command;
        }

        public string Command
        {
            get { return this._command; }
        }
    }
}
