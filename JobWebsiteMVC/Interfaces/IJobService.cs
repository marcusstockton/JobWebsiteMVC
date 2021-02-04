using JobWebsiteMVC.Models.Job;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Interfaces
{
    public interface IJobService
    {
        Task<IList<Job>> GetJobs();

        Task<Job> GetJobById(Guid jobId);

        Task Post(Job job);

        Task Put(Job job);

        Task Delete(Guid jobId);

        Task<List<JobApplication>> GetJobApplicationsForJob(Guid jobId);

        Task<List<JobApplication>> GetJobApplicationsForUser(string userId);

        Task<JobApplication> ApplyForJob(Guid jobId, string userId);

        Task Save();
    }
}