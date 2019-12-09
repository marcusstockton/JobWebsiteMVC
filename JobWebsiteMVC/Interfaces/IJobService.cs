using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobWebsiteMVC.Models.Job;

namespace JobWebsiteMVC.Interfaces
{
    public interface IJobService
    {
        Task<IList<Job>> GetJobs();

        Task<Job> GetJobById(Guid jobId);

        Task Post(Job job);

        Task Put(Job job);

        Task Delete(Guid jobId);

        void Save();
    }
}