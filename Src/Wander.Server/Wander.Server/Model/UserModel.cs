using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server.Model
{
    public class UserModel
    {       
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Sex { get; set; }
    }
}