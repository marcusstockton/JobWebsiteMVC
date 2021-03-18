using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobWebsiteMVC.Models.Job;

namespace JobWebsiteMVC.Interfaces
{
    public interface IJobBenefitsService
    {
        Task<List<JobBenefit>> GetJobBenefits();

        Task<List<Job_JobBenefit>> GetJobBenefitsForJobId(Guid jobId);

        Task CreateOrUpdateJobBenefitsForJob(Guid jobId, List<Job_JobBenefit> currentItems, List<Guid> newItems);
    }
}