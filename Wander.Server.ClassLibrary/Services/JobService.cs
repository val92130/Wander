using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Job;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public class JobService : IJobService
    {
        /// <summary>
        ///     Gets all the info relating the job of the User corresponding to the connectionId
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>Returns a job model containing all the infos</returns>
        public JobModel GetUserJobInfos(string connectionId)
        {
            if (connectionId == null) throw new ArgumentException("there is no id");
            var user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            return GetUserJobInfos(user.UserId);
        }

        public JobModel GetUserJobInfos(int userId)
        {
            if (!ServiceProvider.GetUserService().UserExists(userId)) return null;

            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "SELECT * from dbo.Users u JOIN dbo.Jobs j on j.JobId = u.JobId WHERE u.UserId = @id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@id", userId);

                    var model = new JobModel();

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
        ///     Gets all the info relating the job of the User corresponding to the provided ServerPlayerModel
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>Returns a job model containing all the infos</returns>
        public JobModel GetUserJobInfos(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            return GetUserJobInfos(user.SignalRId);
        }


        /// <summary>
        ///     Gets a list of every possible jobs
        /// </summary>
        /// <returns></returns>
        public List<JobModel> GetAllJobs()
        {
            var jobs = new List<JobModel>();
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "SELECT * from dbo.Jobs";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var model = new JobModel();
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
        ///     Change the Job of a User
        /// </summary>
        /// <param name="jobId">The new job Id</param>
        /// <param name="connectionId">The connection id of the user</param>
        /// <returns>True if success, else false</returns>
        public bool ChangeUserJob(int jobId, string connectionId)
        {
            if (connectionId == null)
                throw new ArgumentException("Connection id is null !");

            var user = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (user == null) throw new ArgumentException("parameter user is null");

            return ChangeUserJob(jobId, user.UserId);
        }

        public bool ChangeUserJob(int jobId, int userId)
        {
            var job = GetAllJobs().FirstOrDefault(x => x.JobId == jobId);

            if (job == null)
            {
                return false;
            }


            if (job.NecessaryPoints > ServiceProvider.GetUserService().GetUserPoints(userId))
            {
                return false;
            }


            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "UPDATE dbo.Users SET JobId = @Id WHERE UserId = @UserId";
                var verifThresholdQuery = "SELECT COUNT(*) FROM dbo.Users u WHERE u.JobId = @Id";
                var count = 0;
                using (var cmd = new SqlCommand(verifThresholdQuery, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", jobId);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }

                if (count >= job.Threshold)
                {
                    return false;
                }

                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", jobId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    var lgn = cmd.ExecuteNonQuery();
                    conn.Close();

                    return lgn != 0;
                }
            }
        }


        /// <summary>
        ///     Change the Job of a User
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

            using (var conn = SqlConnectionService.GetConnection())
            {
                var query =
                    "INSERT INTO dbo.Jobs (JobDescription, Salary, Threshold, EarningPoints, NecessaryPoints) OUTPUT INSERTED.JobId values (@Des, @Salary, @Thres, @Earn, @Necessary)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Des", model.JobDescription);
                    cmd.Parameters.AddWithValue("@Salary", model.Salary);
                    cmd.Parameters.AddWithValue("@Thres", model.Threshold);
                    cmd.Parameters.AddWithValue("@Earn", model.EarningPoints);
                    cmd.Parameters.AddWithValue("@Necessary", model.NecessaryPoints);

                    var lgn = (int) cmd.ExecuteScalar();

                    conn.Close();

                    return lgn;
                }
            }
        }

        /// <summary>
        ///     Delete the job corresponding to the specified JobModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteJob(JobModel model)
        {
            if (model == null) throw new ArgumentException("parameter model is null");

            return DeleteJob(model.JobId);
        }

        public ServerNotificationMessage BuyDrugs(string seller, string buyer)
        {
            if (seller == null) throw new ArgumentException("parameter user is null");
            if (buyer == null) throw new ArgumentException("parameter user2 is null");

            var message = new ServerNotificationMessage();
            message.MessageType = EMessageType.error;
            message.Content = "Error";

            using (var conn = SqlConnectionService.GetConnection())
            {
                var idSeller = ServiceProvider.GetPlayerService().GetPlayer(seller).UserId;
                var idBuyer = ServiceProvider.GetPlayerService().GetPlayer(buyer).UserId;
                var moneyBuyer = ServiceProvider.GetUserService().GetUserBankAccount(buyer);
                var moneySeller = ServiceProvider.GetUserService().GetUserBankAccount(seller);

                var DealerJob = ServiceProvider.GetJobService().GetUserJobInfos(seller).JobDescription;
                if (DealerJob == "Dealer" && moneyBuyer > 30)
                {
                    var remainingMoneySeller = moneySeller + 30;
                    var remainingMoneyBuyer = moneyBuyer - 30;
                    ServiceProvider.GetUserService().SetUserBankAccount(seller, remainingMoneySeller);
                    ServiceProvider.GetUserService().SetUserBankAccount(buyer, remainingMoneyBuyer);

                    message.MessageType = EMessageType.success;
                    message.Content = "Success buying drugs";
                }
                else if (moneyBuyer < 30)
                {
                    message.MessageType = EMessageType.error;
                    message.Content = "You do not have enough money";
                }
            }

            return message;
        }

        /// <summary>
        ///     Delete the job corresponding to the specified JobId
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public bool DeleteJob(int jobId)
        {
            // We put the user unemployed if he currently has the job we wish to delete
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "UPDATE dbo.Users SET JobId = 0 WHERE JobId = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", jobId);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "DELETE from dbo.Jobs WHERE JobId = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", jobId);

                    var lgn = cmd.ExecuteNonQuery();
                    conn.Close();

                    return lgn != 0;
                }
            }
        }
    }
}