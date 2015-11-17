using System.Collections.Generic;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public interface IJobService
    {
        /// <summary>
        /// Gets all the info relating the job of the User corresponding to the connectionId
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>Returns a job model containing all the infos</returns>
        JobModel GetUserJobInfos(string connectionId);

        /// <summary>
        /// Gets all the info relating the job of the User corresponding to the provided ServerPlayerModel
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>Returns a job model containing all the infos</returns>
        JobModel GetUserJobInfos(ServerPlayerModel user);

        /// <summary>
        /// Gets a list of every possible jobs
        /// </summary>
        /// <returns></returns>
        List<JobModel> GetAllJobs();

        /// <summary>
        /// Change the Job of a User
        /// </summary>
        /// <param name="jobId">The new job Id</param>
        /// <param name="connectionId">The connection id of the user</param>
        /// <returns>True if success, else false</returns>
        bool ChangeUserJob(int jobId, string connectionId);

        /// <summary>
        /// Change the Job of a User
        /// </summary>
        /// <param name="jobId">The new job Id</param>
        /// <param name="user">The user</param>
        /// <returns>True if success, else false</returns>
        bool ChangeUserJob(int jobId, ServerPlayerModel user);

        int AddJob(JobModel model);

        /// <summary>
        /// Delete the job corresponding to the specified JobModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool DeleteJob(JobModel model);

        /// <summary>
        /// Delete the job corresponding to the specified JobId
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        bool DeleteJob(int jobId);
    }
}