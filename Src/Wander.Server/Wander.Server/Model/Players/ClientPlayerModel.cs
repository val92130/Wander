using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server.Model
{
    public class ClientPlayerModel
    {
        public Vector2 Position { get; set; }
        public string UserName { get; set; }
        public int Sex { get; set; }
    }
}