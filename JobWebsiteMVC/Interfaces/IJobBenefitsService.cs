using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobWebsiteMVC.Models.Job;

namespace JobWebsiteMVC.Interfaces
{
    public interface IJobBenefitsService
    {
        Task<List<Benefit>> GetJobBenefits();

        Task<List<JobBenefit>> GetJobBenefitsForJobId(Guid jobId);

        Task CreateOrUpdateJobBenefitsForJob(Guid jobId, List<JobBenefit> currentItems, List<Guid> newItems);
    }
}