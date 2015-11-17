using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public interface IJobService
    {
        JobModel GetUserJobInfos(string connectionId);
        JobModel GetUserJobInfos(ServerPlayerModel user);
        List<JobModel> GetAllJobs();
        bool ChangeUserJob(int jobId, string connectionId);
        bool ChangeUserJob(int jobId, ServerPlayerModel user);
        int AddJob(JobModel model);
        bool DeleteJob(JobModel model);
        bool DeleteJob(int jobId);
    }
}