using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server.Model
{
    public class JobModel
    {
        public int JobId { get; set; }
        public string JobDescription { get; set; }
        public int Salary { get; set; }
        public int Threshold { get; set; }
        public int NecessaryPoints { get; set; }
        public int EarningPoints { get; set; }
    }
}