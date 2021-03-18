using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using Microsoft.EntityFrameworkCore;

namespace JobWebsiteMVC.Services
{
    public class JobTypesService : IJobTypesService
    {
        private ApplicationDbContext _context;

        public JobTypesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobType>> GetJobTypes()
        {
            return await _context.JobTypes.ToListAsync();
        }

        public async Task<JobType> GetJobTypeById(Guid id)
        {
            return await _context.JobTypes.FindAsync(id);
        }

        public async Task CreateJobType(JobType jobType)
        {
            await _context.JobTypes.AddAsync(jobType);
            await _context.SaveChangesAsync();
        }

        public async Task<JobType> UpdateJobType(JobType jobType)
        {
            jobType.UpdatedDate = DateTime.Now;
            _context.Update(jobType);
            await _context.SaveChangesAsync();
            return jobType;
        }

        public async Task DeleteJobType(JobType jobType)
        {
            _context.Remove(jobType);
            await _context.SaveChangesAsync();
        }
    }
}