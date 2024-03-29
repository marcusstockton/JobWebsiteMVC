using JobWebsiteMVC.Data;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Services
{
    public class JobService : IJobService, IDisposable
    {
        private ApplicationDbContext _context;
        private readonly IEmailSender _emailService;

        public JobService(ApplicationDbContext context, IEmailSender emailService)
        {
            _context = context;
            _emailService = emailService;
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
                .Include(x => x.JobBenefits)
                    .ThenInclude(x => x.Benefit)
                .Include(x => x.JobType)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public IQueryable<Job> GetJobs(string searchString, bool showExpiredJobs, Guid? jobTypeId = null)
        {
            var jobs = _context.Jobs
                .Include(x => x.JobType)
                .Where(x => x.IsActive);

            if (!string.IsNullOrEmpty(searchString))
            {
                jobs = jobs.Where(x => x.JobTitle.ToLower().Contains(searchString.ToLower()) || x.Description.ToLower().Contains(searchString.ToLower()));
            }
            if (jobTypeId.HasValue)
            {
                jobs = jobs.Where(x => x.JobTypeId == jobTypeId);
            }
            if (showExpiredJobs)
            {
                jobs = jobs.Where(x => DateTimeOffset.Compare(x.ClosingDate, DateTime.Now) < 0);
            }
            else
            {
                jobs = jobs.Where(x => DateTimeOffset.Compare(x.ClosingDate, DateTime.Now) > 0);
            }

            return jobs;
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
            return await _context.JobApplications.Where(x => x.JobId == jobId).ToListAsync();
        }

        public async Task<List<JobApplication>> GetJobApplicationsForUser(string userId)
        {
            return await _context.JobApplications.Where(x => x.ApplicantId == userId).ToListAsync();
        }

        public async Task<JobApplication> ApplyForJob(Guid jobId, string userId)
        {
            var applicant = _context.Users.Find(userId);
            var application = new JobApplication
            {
                ApplicantId = userId,
                JobId = jobId,
                CreatedDate = DateTime.Now,
                IsActive = true,
                CreatedBy = applicant
            };
            await _context.AddAsync(application);
            await Save();

            var job = _context.Jobs.Find(jobId);
            await _emailService.SendEmailAsync(job.CreatedBy.Email, $"You have received an application for job {job.JobTitle}", "<p>You have received an application for job " + job.JobTitle + "</p>");
            await _emailService.SendEmailAsync(applicant.Email, $"You have applied for job {job.JobTitle}", "<p>Congratz!, You have applied for job " + job.JobTitle + "</p>");
            return application;
        }

        public  IQueryable<Job> GetMyJobs(string userId, string searchString, bool showExpiredJobs, Guid? jobTypeId = null)
        {
            var jobs = _context.Jobs
                .Include(x => x.JobType)
                .Where(x => x.CreatedBy.Id == userId);

            if (!string.IsNullOrEmpty(searchString))
            {
                jobs = jobs.Where(x => x.JobTitle.ToLower().Contains(searchString.ToLower()) || x.Description.ToLower().Contains(searchString.ToLower()));
            }
            if (jobTypeId.HasValue)
            {
                jobs = jobs.Where(x => x.JobTypeId == jobTypeId);
            }
            if (showExpiredJobs)
            {
                jobs = jobs.Where(x => DateTimeOffset.Compare(x.ClosingDate, DateTime.Now) < 0);
            }
            else
            {
                jobs = jobs.Where(x => DateTimeOffset.Compare(x.ClosingDate, DateTime.Now) > 0);
            }

            return jobs;
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