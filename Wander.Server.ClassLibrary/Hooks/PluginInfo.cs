using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander.Server.ClassLibrary.Hooks
{
    public class PluginInfo : Attribute
    {
        private string _name, _description, _author, _version;
        public PluginInfo(string name, string description, string author,string version)
        {
            this._name = name;
            this._description = description;
            this._author = author;
            this._version = version;
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public string Description
        {
            get { return this._description; }
        }

        public string Author
        {
            get { return this._author; }
        }

        public string Version
        {
            get { return this._version; }
        }
    }
}
