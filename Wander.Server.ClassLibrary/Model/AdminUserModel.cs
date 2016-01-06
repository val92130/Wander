using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.ClassLibrary.Model.Job;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Model
{
    public class AdminUserModel
    {
        public int UserId { get; set; }
        public string Pseudo { get; set; }
        public Vector2 Position { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }
        public int Account { get; set; }
        public int Points { get; set; }
        public JobModel Job { get; set; }
        public List<ServerPropertyModel> Properties { get; set; }
        public bool IsBanned { get; set; }
    }
}
