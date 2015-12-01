using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Wander.Server.ClassLibrary.Model.Job;

namespace Wander.Server.ClassLibrary.Services
{
    class QuestionService : IQuestionService
    {
        public JobQuestionModel GetRandomQuestion(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");

            int jobId = ServiceProvider.GetJobService().GetUserJobInfos(ConnectionId).JobId;
            JobQuestionModel randomQuestion = new JobQuestionModel();
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT top 1 * FROM  dbo.Questions ORDER BY  newid()";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        JobQuestionModel questionModel = new JobQuestionModel();
                        questionModel.QuestionId = Convert.ToInt32(reader["QuestionId"]);
                        questionModel.Question = reader["Question"].ToString();
                        questionModel.JobId = Convert.ToInt32(reader["JobId"]);
                    }

                    conn.Close();
                    return randomQuestion;
                }
            }
        }

        public bool CheckAnswer(JobQuestionModel questionModel, bool validAnswer)
        {
            bool answer = questionModel.Answer;
            int idQuestion = questionModel.QuestionId;
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT Answer FROM  dbo.Questions WHERE QuestionId = @idQuestion ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        questionModel.Answer = (bool)(reader["Answer"]);
                    }

                    conn.Close();
                    return questionModel.Answer == answer;
                }
            }
        }

        public List<JobQuestionModel> GetAllQuestions()
        {
            List<JobQuestionModel> questions = new List<JobQuestionModel>();
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT * from dbo.Questions";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        JobQuestionModel questionModel = new JobQuestionModel();
                        questionModel.Question = reader["Question"].ToString();
                        questions.Add(questionModel);
                    }
                    conn.Close();
                    return questions;
                }
            }
        }
    }
}
