using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server.Model
{
    /// <summary>
    /// Represent a Wander Game Player from the Server Side, these objects should NOT be transfered to the clients
    /// </summary>
    public class ServerPlayerModel
    {
        public int UserId { get; set; }
        public string SignalRId { get; set; }
        public Vector2 Position { get; set; }
    }
}