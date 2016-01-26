using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Wander.Server.ClassLibrary.Model.Job;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    internal class QuestionService : IQuestionService
    {
        public JobQuestionModel GetRandomQuestion(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");

            return GetRandomQuestion(ServiceProvider.GetPlayerService().GetPlayer(connectionId).UserId);
        }

        public JobQuestionModel GetRandomQuestion(int userId)
        {
            if (!ServiceProvider.GetUserService().UserExists(userId)) return null;

            var jobId = ServiceProvider.GetJobService().GetUserJobInfos(userId).JobId;
            JobQuestionModel randomQuestion = null;
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "SELECT top 1 * FROM  dbo.Questions WHERE dbo.Questions.JobId = @JobId ORDER BY newid()";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@JobId",
                        ServiceProvider.GetJobService().GetUserJobInfos(userId).JobId);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        randomQuestion = new JobQuestionModel();
                        randomQuestion.QuestionId = Convert.ToInt32(reader["QuestionId"]);
                        randomQuestion.Question = reader["Question"].ToString();
                        randomQuestion.Answer = bool.Parse(reader["Answer"].ToString());
                        randomQuestion.JobId = Convert.ToInt32(reader["JobId"]);
                        break;
                    }

                    conn.Close();
                    return randomQuestion;
                }
            }
        }

        public JobQuestionModel GetRandomQuestion(ServerPlayerModel user)
        {
            if (user == null) return null;
            return GetRandomQuestion(user.UserId);
        }

        public bool CheckAnswer(JobQuestionModel questionModel)
        {
            if (questionModel == null) throw new ArgumentNullException("QuestionModel");
            var answer = questionModel.Answer;
            var idQuestion = questionModel.QuestionId;
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "SELECT Answer FROM dbo.Questions WHERE QuestionId = @idQuestion ";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@idQuestion", idQuestion);
                    var data = cmd.ExecuteScalar();
                    if (data == null) throw new ArgumentNullException("Question not found");

                    var t = Convert.ToBoolean(data);

                    conn.Close();
                    return t == answer;
                }
            }
        }

        public List<JobQuestionModel> GetAllQuestions()
        {
            var questions = new List<JobQuestionModel>();
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "SELECT * from dbo.Questions";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var questionModel = new JobQuestionModel();
                        questionModel.QuestionId = Convert.ToInt32(reader["QuestionId"]);
                        questionModel.Question = reader["Question"].ToString();
                        questionModel.Answer = bool.Parse(reader["Answer"].ToString());
                        questionModel.JobId = Convert.ToInt32(reader["JobId"]);
                        questions.Add(questionModel);
                    }
                    conn.Close();
                    return questions;
                }
            }
        }
    }
}