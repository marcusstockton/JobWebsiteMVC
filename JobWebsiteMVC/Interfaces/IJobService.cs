using JobWebsiteMVC.Models.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Interfaces
{
    public interface IJobService
    {
        IQueryable<Job> GetJobs(string searchString, bool showExpiredJobs, Guid? jobTypeId = null);

        Task<Job> GetJobById(Guid jobId);

        Task Post(Job job);

        Task Put(Job job);

        Task Delete(Guid jobId);

        Task<List<JobApplication>> GetJobApplicationsForJob(Guid jobId);

        Task<List<JobApplication>> GetJobApplicationsForUser(string userId);

        Task<JobApplication> ApplyForJob(Guid jobId, string userId);

        IQueryable<Job> GetMyJobs(string userId, string searchString, bool showExpiredJobs, Guid? jobTypeId);

        Task Save();
    }
}