using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server.Model
{
    public class ServerNotificationMessage
    {
        public string Content { get; set; }
        public EMessageType MessageType { get; set; }
    }
}