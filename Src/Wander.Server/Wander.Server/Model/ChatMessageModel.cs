﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server.Model
{
    public class ChatMessageModel
    {
        public string UserName { get; set; }
        public int Sex { get; set; }
        public string Hour { get; set; }
        public string Content { get; set; }
    }
}