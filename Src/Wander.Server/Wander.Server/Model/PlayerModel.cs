using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server.Model
{
    /// <summary>
    /// Represent a Wander Game Player
    /// </summary>
    public class PlayerModel
    {
        public int UserId { get; set; }
        public string SignalRId { get; set; }
    }
}