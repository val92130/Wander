using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model.Job;

namespace Wander.Server.ClassLibrary.Services
{
    public interface IQuestionService
    {
        bool CheckAnswer(JobQuestionModel questionModel);
        List<JobQuestionModel> GetAllQuestions();
        JobQuestionModel GetRandomQuestion(string ConnectionId);
    }
}