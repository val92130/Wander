using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model.Job;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public interface IQuestionService
    {
        bool CheckAnswer(JobQuestionModel questionModel);
        List<JobQuestionModel> GetAllQuestions();
        JobQuestionModel GetRandomQuestion(string connectionId);
        JobQuestionModel GetRandomQuestion(int userId);
        JobQuestionModel GetRandomQuestion(ServerPlayerModel user);
    }
}