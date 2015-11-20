using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Wander.Server.Hubs;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public class JobService : IJobService
    {
        /// <summary>
        /// Gets all the info relating the job of the User corresponding to the connectionId
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>Returns a job model containing all the infos</returns>
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
                        model.EarningPoints = Convert.ToInt32(reader["EarningPoints"]);
                        model.NecessaryPoints = Convert.ToInt32(reader["NecessaryPoints"]);
                    }

                    conn.Close();

                    return model;
                }
            }
        }

        /// <summary>
        /// Gets all the info relating the job of the User corresponding to the provided ServerPlayerModel
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>Returns a job model containing all the infos</returns>
        public JobModel GetUserJobInfos(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserJobInfos(user.SignalRId);
        }

        /// <summary>
        /// Gets a list of every possible jobs
        /// </summary>
        /// <returns></returns>
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
                        model.EarningPoints = Convert.ToInt32(reader["EarningPoints"]);
                        model.NecessaryPoints = Convert.ToInt32(reader["NecessaryPoints"]);
                        jobs.Add(model);
                    }

                    conn.Close();
                    return jobs;
                }
            }
        }

        /// <summary>
        /// Change the Job of a User
        /// </summary>
        /// <param name="jobId">The new job Id</param>
        /// <param name="connectionId">The connection id of the user</param>
        /// <returns>True if success, else false</returns>
        public bool ChangeUserJob(int jobId, string connectionId)
        {
            if(connectionId == null)
                throw new ArgumentException("Connection id is null !");

            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            JobModel job = GetAllJobs().FirstOrDefault(x => x.JobId == jobId);

            if (job == null)
            {
                return false;
            }


            if (job.NecessaryPoints > ServiceProvider.GetUserService().GetUserPoints(user))
            {
                return false;
            }

            
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "UPDATE dbo.Users SET JobId = @Id WHERE UserId = @UserId";
                string verifThresholdQuery = "SELECT COUNT(*) FROM dbo.Users u WHERE u.JobId = @Id";
                int count = 0;
                using (SqlCommand cmd = new SqlCommand(verifThresholdQuery, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", jobId);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }

                if (count >= job.Threshold)
                {
                    GlobalHost.ConnectionManager.GetHubContext<GameHub>()
                        .Clients.Client(connectionId)
                        .notify(Helper.CreateNotificationMessage("This job is already full !", EMessageType.error));
                    return false;
                }

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


        /// <summary>
        /// Change the Job of a User
        /// </summary>
        /// <param name="jobId">The new job Id</param>
        /// <param name="user">The user</param>
        /// <returns>True if success, else false</returns>
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
                string query = "INSERT INTO dbo.Jobs (JobDescription, Salary, Threshold, EarningPoints, NecessaryPoints) OUTPUT INSERTED.JobId values (@Des, @Salary, @Thres, @Earn, @Necessary)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Des", model.JobDescription);
                    cmd.Parameters.AddWithValue("@Salary", model.Salary);
                    cmd.Parameters.AddWithValue("@Thres", model.Threshold);
                    cmd.Parameters.AddWithValue("@Earn", model.EarningPoints);
                    cmd.Parameters.AddWithValue("@Necessary", model.NecessaryPoints);

                    int lgn = (int)cmd.ExecuteScalar();

                    conn.Close();

                    return lgn;
                }
            }

        }

        /// <summary>
        /// Delete the job corresponding to the specified JobModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteJob(JobModel model)
        {
            if (model == null) throw new ArgumentException("parameter model is null");

            return DeleteJob(model.JobId);
        }

        /// <summary>
        /// Delete the job corresponding to the specified JobId
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
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