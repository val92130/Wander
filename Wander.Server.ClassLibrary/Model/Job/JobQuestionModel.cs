namespace Wander.Server.ClassLibrary.Model.Job
{
    public class JobQuestionModel
    {
        public int QuestionId { get; set; }
        public int JobId { get; set; }
        public string Question { get; set; }
        public bool Answer { get; set; }
    }
}