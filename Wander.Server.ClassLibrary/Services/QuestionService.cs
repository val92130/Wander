using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using Wander.Server.ClassLibrary.Model.Job;

namespace Wander.Server.ClassLibrary.Services
{
    class QuestionService : IQuestionService
    {
        public JobQuestionModel GetRandomQuestion(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");

            int jobId = ServiceProvider.GetJobService().GetUserJobInfos(ConnectionId).JobId;
            JobQuestionModel randomQuestion = null;
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT top 1 * FROM  dbo.Questions WHERE dbo.Questions.JobId = @JobId ORDER BY newid()";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@JobId",
                        ServiceProvider.GetJobService().GetUserJobInfos(ConnectionId).JobId);
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

        public bool CheckAnswer(JobQuestionModel questionModel)
        {
            if (questionModel == null) throw new ArgumentNullException("QuestionModel");
            bool answer = questionModel.Answer;
            int idQuestion = questionModel.QuestionId;
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT Answer FROM dbo.Questions WHERE QuestionId = @idQuestion ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@idQuestion", idQuestion);
                    var data = cmd.ExecuteScalar();
                    if (data == null) throw new ArgumentNullException("Question not found");

                    bool t = Convert.ToBoolean(data);

                    conn.Close();
                    return t == answer;
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
