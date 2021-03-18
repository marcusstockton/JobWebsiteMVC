using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobWebsiteMVC.Models.Job;

namespace JobWebsiteMVC.Interfaces
{
    public interface IJobTypesService
    {
        Task<List<JobType>> GetJobTypes();

        Task<JobType> GetJobTypeById(Guid id);

        Task CreateJobType(JobType jobType);

        Task<JobType> UpdateJobType(JobType jobType);

        Task DeleteJobType(JobType jobType);
    }
}