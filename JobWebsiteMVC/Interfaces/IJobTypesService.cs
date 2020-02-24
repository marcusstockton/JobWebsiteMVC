using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobWebsiteMVC.Models.Job;

namespace JobWebsiteMVC.Interfaces
{
    public interface IJobTypesService
    {
        Task<List<JobType>> GetJobTypes();
    }
}
