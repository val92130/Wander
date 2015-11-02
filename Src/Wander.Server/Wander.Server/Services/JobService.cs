using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public class JobService : IJobService
    {
        public JobModel GetUserJobInfos(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT * from dbo.Users u JOIN dbo.Jobs j on j.JobId = u.JobId WHERE u.UserId = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@id", user.UserId);

                    JobModel model = new JobModel();

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        model.JobDescription = reader["JobDescription"].ToString();
                        model.JobId = Convert.ToInt32(reader["JobId"]);
                        model.Salary = Convert.ToInt32(reader["Salary"]);
                        model.Threshold = Convert.ToInt32(reader["Threshold"]);
                    }

                    conn.Close();

                    return model;
                }
            }
        }

        public JobModel GetUserJobInfos(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserJobInfos(user.SignalRId);
        }

        public List<JobModel> GetAllJobs()
        {
            List<JobModel> jobs = new List<JobModel>();
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT * from dbo.Jobs";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        JobModel model = new JobModel();
                        model.JobDescription = reader["JobDescription"].ToString();
                        model.JobId = Convert.ToInt32(reader["JobId"]);
                        model.Salary = Convert.ToInt32(reader["Salary"]);
                        model.Threshold = Convert.ToInt32(reader["Threshold"]);
                        jobs.Add(model);
                    }

                    conn.Close();
                    return jobs;
                }
            }
        }

        public bool ChangeUserJob(int jobId, string connectionId)
        {
            if(connectionId == null)
                throw new ArgumentException("Connection id is null !");

            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT JobDescription from dbo.Jobs WHERE JobId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", jobId);

                    int lgn = cmd.ExecuteNonQuery();
                    
                    conn.Close();

                    if (lgn == 0)
                    {
                        return false;
                    }
                }
            }

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "UPDATE dbo.Users SET JobId = @Id WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", jobId);
                    cmd.Parameters.AddWithValue("@UserId", user.UserId);

                    int lgn = cmd.ExecuteNonQuery();
                    conn.Close();

                    return lgn != 0;
                }
            }


        }

        public bool ChangeUserJob(int jobId, ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return ChangeUserJob(jobId, user.SignalRId);
        }

        public int AddJob(JobModel model)
        {
            if (model == null) throw new ArgumentException("parameter model is null");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "INSERT INTO dbo.Jobs (JobDescription, Salary, Threshold) OUTPUT INSERTED.JobId values (@Des, @Salary, @Thres)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Des", model.JobDescription);
                    cmd.Parameters.AddWithValue("@Salary", model.Salary);
                    cmd.Parameters.AddWithValue("@Thres", model.Threshold);

                    int lgn = (int)cmd.ExecuteScalar();

                    conn.Close();

                    return lgn;
                }
            }

        }

        public bool DeleteJob(JobModel model)
        {
            if (model == null) throw new ArgumentException("parameter model is null");

            return DeleteJob(model.JobId);
        }

        public bool DeleteJob(int jobId)
        {
            // We put the user unemployed if he currently has the job we wish to delete
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "UPDATE dbo.Users SET JobId = 0 WHERE JobId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", jobId);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "DELETE from dbo.Jobs WHERE JobId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", jobId);

                    int lgn = cmd.ExecuteNonQuery();
                    conn.Close();

                    return lgn != 0;
                }
            }
        }

    }
}