using System;
using System.Collections.Generic;
using System.Linq;
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
            await Save();
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
            job.CreatedDate = DateTime.Now;
            await _context.Jobs.AddAsync(job);
            await Save();
        }

        public async Task Put(Job job)
        {
            job.UpdatedDate = DateTime.Now;
            _context.Entry(job).State = EntityState.Modified;
            await Save();
        }

        public async Task<List<JobApplication>> GetJobApplicationsForJob(Guid jobId)
        {
            return await _context.JobApplications.Where( x => x.JobId == jobId ).ToListAsync();
        }

        public async Task<List<JobApplication>> GetJobApplicationsForUser(string userId)
        {
            return await _context.JobApplications.Where( x => x.ApplicantId == userId ).ToListAsync();
        }

        public async Task<JobApplication> ApplyForJob(Guid jobId, string userId)
        {
            var application = new JobApplication
            {
                ApplicantId = userId,
                JobId = jobId,
                CreatedDate = DateTime.Now,
                IsActive = true,
                CreatedBy = _context.Users.Find( userId )
            };
            await _context.AddAsync( application );
            await Save();
            return application;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
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