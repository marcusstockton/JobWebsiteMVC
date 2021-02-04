using JobWebsiteMVC.Models.Job;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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