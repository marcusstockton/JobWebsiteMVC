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
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailService;

        public JobService(ApplicationDbContext context, IUnitOfWork unitOfWork, IEmailSender emailService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task Delete(Guid id)
        {
            var job = await _unitOfWork.Jobs.GetById(id);
            _unitOfWork.Jobs.Delete(job.Id);
            //await Save();
        }

        public async Task<Job> GetJobById(Guid id)
        {
            return await _unitOfWork.Jobs.GetById(id);
            //return await _context.Jobs
            //    .Include(x => x.JobBenefits)
            //        .ThenInclude(x => x.Benefit)
            //    .Include(x => x.JobType)
            //    .FirstOrDefaultAsync(m => m.Id == id);
        }

        public IQueryable<Job> GetJobs(string searchString, bool showExpiredJobs, Guid? jobTypeId = null)
        {
            var jobs = _unitOfWork.Jobs.GetAsQueryable(includeProperties: "JobType", filter: x => x.IsActive);

            if (!string.IsNullOrEmpty(searchString))
            {
                // jobs = jobs.Where(x => x.JobTitle.ToLower().Contains(searchString.ToLower()) || x.Description.ToLower().Contains(searchString.ToLower()));
                jobs = JobFullTextSearchWithRank(searchString);
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

        public async Task Post(Job job, string creatorId)
        {
            var user = await _context.Users.FindAsync(creatorId);
            job.CreatedBy = user;
            await _unitOfWork.Jobs.Add(job);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Put(Job job)
        {
            _unitOfWork.Jobs.Update(job);
            await _unitOfWork.CompleteAsync();
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

        public IQueryable<Job> GetMyJobs(string userId, string searchString, bool showExpiredJobs, Guid? jobTypeId = null)
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

        public IQueryable<Job> JobFullTextSearchWithRank(string searchTerm)
        {
            var jobs = _context.Jobs
                .Where(
                    b => EF.Functions.ToTsVector("english", b.JobTitle + " " + b.Description)
                    .Matches(EF.Functions.PhraseToTsQuery("english", searchTerm)))
                    .Select(b => new
                    {
                        b.Id,
                        b.CreatedDate,
                        b.Description,
                        b.HolidayEntitlement,
                        b.HoursPerWeek,
                        b.IsActive,
                        b.IsDraft,
                        b.JobTitle,
                        b.MaxSalary,
                        b.MinSalary,
                        b.PublishDate,
                        b.ClosingDate,
                        b.WorkingHoursEnd,
                        b.WorkingHoursStart,
                        b.CreatedBy,
                        b.UpdatedDate,
                        b.JobCategories,
                        b.JobTypeId,
                        b.JobType,
                        b.JobBenefits,
                        Rank = EF.Functions.ToTsVector("english", b.JobTitle + " " + b.Description)
                            .Rank(EF.Functions.PhraseToTsQuery("english", searchTerm))
                    })
                    .OrderByDescending(x => x.Rank)
                    .AsQueryable()
                    .Select(b => new Job
                    {
                        Id = b.Id,
                        HolidayEntitlement = b.HolidayEntitlement,
                        JobCategories = b.JobCategories,
                        HoursPerWeek = b.HoursPerWeek,
                        IsDraft = b.IsDraft,
                        JobTitle = b.JobTitle,
                        MaxSalary = b.MaxSalary,
                        MinSalary = b.MinSalary,
                        PublishDate = b.PublishDate,
                        ClosingDate = b.ClosingDate,
                        WorkingHoursEnd = b.WorkingHoursEnd,
                        Description = b.Description,
                        JobBenefits = _context.JobBenefits.Include(x => x.Benefit).Where(x => x.JobId == b.Id).ToList(),
                        WorkingHoursStart = b.WorkingHoursStart,
                        CreatedDate = b.CreatedDate,
                        JobTypeId = b.JobTypeId,
                        JobType = b.JobType,
                        CreatedBy = b.CreatedBy,
                        IsActive = b.IsActive,
                        UpdatedDate = b.UpdatedDate
                    })
                    .AsQueryable();
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