using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using Microsoft.EntityFrameworkCore;

namespace JobWebsiteMVC.Services
{
    public class JobService : IJobService, IDisposable
    {
        private ApplicationDbContext _context;
        public JobService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Delete(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            _context.Jobs.Remove(job);            
        }
       
        public async Task<Job> GetJobById(Guid id)
        {
            return await _context.Jobs
                .Include(x => x.Job_JobBenefits)
                .ThenInclude(x=>x.JobBenefit)
                .Include(x=>x.JobType)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IList<Job>> GetJobs()
        {
            return await _context.Jobs
                .Include(x=>x.JobType)
                .ToListAsync();
        }

        public async Task Post(Job job)
        {
            await _context.Jobs.AddAsync(job);
        }

        public async Task Put(Job job)
        {
            await Task.Run(() =>
            {
                _context.Entry(job).State = EntityState.Modified;
            });
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}