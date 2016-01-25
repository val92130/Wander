namespace Wander.Server.ClassLibrary.Model.Job
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