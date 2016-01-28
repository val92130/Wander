using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander.Server.ClassLibrary.Hooks
{
    public class ChatCommand : System.Attribute
    {
        private string _command, _info;
        public ChatCommand(string command, string info)
        {
            this._command = command;
            this._info = info;
        }

        public ChatCommand(string command)
        {
            this._command = command;
            this._info = "No info available";
        }

        public string Command
        {
            get { return this._command; }
        }

        public string Info
        {
            get { return this._info; }
        }
    }
}
