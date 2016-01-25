using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model.Job;

namespace Wander.Server.ClassLibrary.Model.Players
{
    public class AdminPlayerModel : ServerPlayerModel
    {
        public string Email { get; set; }
        public int Account { get; set; }
        public int Points { get; set; }
        public JobModel Job { get; set; }
        public List<ServerPropertyModel> Properties { get; set; }
        public bool IsBanned { get; set; }
    }
}