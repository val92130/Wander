using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model.Job;

namespace Wander.Server.ClassLibrary.Services
{
    interface IQuestionService
    {
        bool CheckAnswer(JobQuestionModel questionModel, bool validAnswer);
        List<JobQuestionModel> GetAllQuestions();
        JobQuestionModel GetRandomQuestion(string ConnectionId);
    }
}