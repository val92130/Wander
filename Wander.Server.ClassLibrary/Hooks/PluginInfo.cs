using System;

namespace Wander.Server.ClassLibrary.Hooks
{
    public class PluginInfo : Attribute
    {
        public PluginInfo(string name, string description, string author, string version)
        {
            Name = name;
            Description = description;
            Author = author;
            Version = version;
        }

        public string Name { get; }

        public string Description { get; }

        public string Author { get; }

        public string Version { get; }
    }
}