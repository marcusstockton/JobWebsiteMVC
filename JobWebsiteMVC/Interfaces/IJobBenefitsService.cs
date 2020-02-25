﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobWebsiteMVC.Models.Job;

namespace JobWebsiteMVC.Interfaces
{
    public interface IJobBenefitsService
    {
        Task<List<JobBenefit>> GetJobBenefits();

        Task<List<Job_JobBenefit>> GetJobBenefitsForJobId(Guid jobId);
        void CreateOrUpdateJobBenefitsForJob(Guid jobId, List<Job_JobBenefit> currentItems, List<Guid> newItems);
    }
}