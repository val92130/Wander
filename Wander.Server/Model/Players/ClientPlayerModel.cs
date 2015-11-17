using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wander.Server.Model.Players;

namespace Wander.Server.Model
{
    public class ClientPlayerModel
    {
        public Vector2 Position { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }
        public int Account { get; set; }
        public int Points { get; set; }
        public JobModel Job { get; set; }
        public List<ServerPropertyModel> Properties { get; set; } 
    }
}