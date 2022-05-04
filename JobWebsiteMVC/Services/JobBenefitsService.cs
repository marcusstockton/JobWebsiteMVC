using JobWebsiteMVC.Data;
using JobWebsiteMVC.Extensions;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Services
{
    public class JobBenefitsService : IJobBenefitsService
    {
        private ApplicationDbContext _context;

        public JobBenefitsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Benefit>> GetJobBenefits()
        {
            return await _context.Benefits.ToListAsync();
        }

        public async Task<List<JobBenefit>> GetJobBenefitsForJobId(Guid jobId)
        {
            return await _context.JobBenefits.Where(x => x.JobId == jobId).ToListAsync();
        }

        // https://stackoverflow.com/questions/42993860/entity-framework-core-update-many-to-many
        public async Task CreateOrUpdateJobBenefitsForJob(Guid jobId, List<JobBenefit> currentItems, List<Guid> newItems)
        {
            _context.TryUpdateManyToMany(currentItems, newItems
            .Select(x => new JobBenefit
            {
                JobId = jobId,
                JobBenefitId = x
            }), x => x);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}