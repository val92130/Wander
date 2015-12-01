using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Job;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public interface IJobService
    {
        int AddJob(JobModel model);
        ServerNotificationMessage BuyDrugs(string seller, string buyer);
        bool ChangeUserJob(int jobId, string connectionId);
        bool ChangeUserJob(int jobId, ServerPlayerModel user);
        bool DeleteJob(JobModel model);
        bool DeleteJob(int jobId);
        List<JobModel> GetAllJobs();
        JobModel GetUserJobInfos(string connectionId);
        JobModel GetUserJobInfos(ServerPlayerModel user);
    }
}