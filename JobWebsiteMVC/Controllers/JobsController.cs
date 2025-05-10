using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Extensions.Alerts;
using JobWebsiteMVC.Helpers;
using JobWebsiteMVC.Hubs;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using JobWebsiteMVC.ViewModels.Job;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Controllers
{
    public class JobsController : Controller
    {
        private readonly ILogger<JobsController> _logger;
        private readonly IJobService _service;
        private readonly IJobTypesService _jobTypesService;
        private readonly IJobBenefitsService _jobBenefitsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationsHub, INotificationsHub> _hubContext;

        public JobsController(ILogger<JobsController> logger, IJobService service, IJobTypesService jobTypesService,
            IJobBenefitsService jobBenefitsService, IUnitOfWork unitOfWork, IHubContext<NotificationsHub, INotificationsHub> hubContext)
        {
            _logger = logger;
            _service = service;
            _jobTypesService = jobTypesService;
            _jobBenefitsService = jobBenefitsService;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        // GET: Jobs
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, bool showExpiredJobs, int? pageNumber, Guid? jobTypeId = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["JobTitleSortParm"] = sortOrder == "job_title_asc" ? "job_title_desc" : "job_title_asc";
            ViewData["JobCreatedSortParm"] = sortOrder == "job_created_asc" ? "job_created_desc" : "job_created_asc";
            ViewData["jobTypeId"] = jobTypeId;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var jobList = _service.GetJobs(searchString, showExpiredJobs, jobTypeId).OrderByDescending(x => x.CreatedDate);

            var jobTypes = await _jobTypesService.GetJobTypes();
            ViewData["JobTypes"] = jobTypes.OrderBy(x => x.Description).Where(x => x.IsActive).ToList();

            switch (sortOrder)
            {
                case "job_title_desc":
                    jobList = jobList.OrderByDescending(x => x.JobTitle);
                    break;

                case "job_title_asc":
                    jobList = jobList.OrderBy(x => x.JobTitle);
                    break;

                case "closing_date_desc":
                    jobList = jobList.OrderByDescending(x => x.ClosingDate);
                    break;

                case "closing_date_asc":
                    jobList = jobList.OrderBy(x => x.ClosingDate);
                    break;
                case "job_created_asc":
                    jobList = jobList.OrderBy(x => x.CreatedDate);
                    break;
                case "job_created_desc":
                    jobList = jobList.OrderByDescending(x => x.CreatedDate);
                    break;
                default:
                    break;
            }

            var jobListViewModels = jobList.Select(job => new JobListViewModel
            {
                Id = job.Id,
                JobTitle = job.JobTitle,
                Description = job.Description,
                HolidayEntitlement = job.HolidayEntitlement.GetValueOrDefault(),
                JobType = job.JobType,
                CreatedDate = job.CreatedDate.DateTime
            }).AsQueryable();

            int pageSize = 10;
            ViewData["totalPages"] = (jobList.Count() / pageSize) + 1;

            return View(await PaginatedList<JobListViewModel>.CreateAsync(jobListViewModels, pageNumber ?? 1, pageSize));
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _service.GetJobById(id.Value);

            if (job == null)
            {
                _logger.LogError($"No Job Found with an Id of {id}");
                return NotFound();
            }

            var jobVM = new JobDetailsViewModel
            {
                Id = job.Id,
                JobTitle = job.JobTitle,
                Description = job.Description,
                MinSalary = job.MinSalary.GetValueOrDefault(),
                MaxSalary = job.MaxSalary.GetValueOrDefault(),
                WorkingHoursStart = job.WorkingHoursStart,
                WorkingHoursEnd = job.WorkingHoursEnd,
                HoursPerWeek = job.HoursPerWeek.GetValueOrDefault(),
                HolidayEntitlement = job.HolidayEntitlement.GetValueOrDefault(),
                ClosingDate = job.ClosingDate,
                PublishDate = job.PublishDate,
                CreatedDate = job.CreatedDate,
                IsActive = job.IsActive,
                JobType = job.JobType,
                IsDraft = job.IsDraft,
                JobBenefits = job.JobBenefits.ToList(),
                UpdatedDate = job.UpdatedDate,
            };

            return View(jobVM);
        }

        // GET: Jobs/Create
        public async Task<IActionResult> Create()
        {
            var job = new JobCreateViewModel();
            var jobBenefits = await _jobBenefitsService.GetJobBenefits();
            var jobTypes = await _jobTypesService.GetJobTypes();
            _logger.LogInformation(jobBenefits.Count() + " job benefits found.");

            ViewBag.JobBenefitsList = jobBenefits.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description }).ToList();
            ViewBag.JobTypesList = jobTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description }).ToList();
            return View(job);
        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,JobOwner")]
        public async Task<IActionResult> Create(JobCreateViewModel jobVM)
        {
            if (ModelState.IsValid)
            {
                var job = new Job
                {
                    Id = Guid.NewGuid(),
                    JobTitle = jobVM.JobTitle,
                    Description = jobVM.Description,
                    IsDraft = jobVM.IsDraft,
                    MinSalary = jobVM.MinSalary,
                    MaxSalary = jobVM.MaxSalary,
                    WorkingHoursStart = jobVM.WorkingHoursStart,
                    WorkingHoursEnd = jobVM.WorkingHoursEnd,
                    HoursPerWeek = jobVM.HoursPerWeek,
                    HolidayEntitlement = jobVM.HolidayEntitlement,
                    ClosingDate = jobVM.ClosingDate,
                    PublishDate = jobVM.PublishDate,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = jobVM.IsActive,
                    JobTypeId = jobVM.JobTypeId
                };

                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _service.Post(job, userid);
                await _jobBenefitsService.CreateOrUpdateJobBenefitsForJob(job.Id, new List<JobBenefit>(), jobVM.JobBenefitsIds);

                return RedirectToAction(nameof(Index)).WithSuccess("Success", "Job successfully created!");
            }
            return View(jobVM).WithDanger("Error", "Some errors occurred creating the job");
        }

        // GET: Jobs/Edit/5
        [Authorize(Roles = "Admin,JobOwner")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var job = await _service.GetJobById(id.Value);

            if (job == null)
            {
                _logger.LogError($"No Job Found with an Id of {id}");
                return NotFound();
            }

            var jobVM = new JobEditViewModel
            {
                Id = job.Id,
                JobTitle = job.JobTitle,
                Description = job.Description,
                IsDraft = job.IsDraft,
                MinSalary = job.MinSalary.GetValueOrDefault(),
                MaxSalary = job.MaxSalary.GetValueOrDefault(),
                WorkingHoursStart = job.WorkingHoursStart,
                WorkingHoursEnd = job.WorkingHoursEnd,
                HoursPerWeek = job.HoursPerWeek.GetValueOrDefault(),
                HolidayEntitlement = job.HolidayEntitlement.GetValueOrDefault(),
                ClosingDate = job.ClosingDate,
                PublishDate = job.PublishDate,
                CreatedDate = job.CreatedDate,
                IsActive = job.IsActive,
                JobTypeId = job.JobTypeId,
                JobBenefitsIds = job.JobBenefits?.Select(s => s.JobBenefitId).ToList()
            };

            var jobTypes = await _jobTypesService.GetJobTypes();
            var jobBenefits = await _jobBenefitsService.GetJobBenefits();
            jobVM.JobTypesList = jobTypes.Select(x => new SelectListItem { Text = x.Description, Value = x.Id.ToString() }).ToList();

            ViewBag.JobBenefits = jobBenefits.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description }).ToList();
            return View(jobVM);
        }

        // POST: Jobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,JobOwner")]
        public async Task<IActionResult> Edit(Guid id, [Bind("JobTitle,Description,IsDraft,MinSalary,MaxSalary,WorkingHoursStart,WorkingHoursEnd,HoursPerWeek,HolidayEntitlement,ClosingDate,PublishDate,Id,CreatedDate,UpdatedDate,IsActive,JobBenefitsIds,JobTypeId")] JobEditViewModel jobVM)
        {
            if (id != jobVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var job = await _service.GetJobById(id);
                    if (job == null)
                    {
                        return NotFound();
                    }

                    job.JobTitle = jobVM.JobTitle;
                    job.Description = jobVM.Description;
                    job.IsDraft = jobVM.IsDraft;
                    job.MinSalary = jobVM.MinSalary;
                    job.MaxSalary = jobVM.MaxSalary;
                    job.WorkingHoursStart = jobVM.WorkingHoursStart;
                    job.WorkingHoursEnd = jobVM.WorkingHoursEnd;
                    job.HoursPerWeek = jobVM.HoursPerWeek;
                    job.HolidayEntitlement = jobVM.HolidayEntitlement;
                    job.ClosingDate = jobVM.ClosingDate;
                    job.PublishDate = jobVM.PublishDate;
                    job.IsActive = jobVM.IsActive;
                    job.JobTypeId = jobVM.JobTypeId;

                    var currentJobBenefits = await _jobBenefitsService.GetJobBenefitsForJobId(job.Id);
                    await _jobBenefitsService.CreateOrUpdateJobBenefitsForJob(job.Id, currentJobBenefits, jobVM.JobBenefitsIds);

                    await _service.Put(job);

                    await _hubContext.Clients.All.SendMessage(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString(), "This is a Test");

                    return RedirectToAction(nameof(Index)).WithSuccess("Success", "Job Updated Successfully!");
                }
                catch (Exception ex)
                {
                    return View(jobVM).WithDanger("Error", ex.Message);
                }
            }

            var jobTypes = await _jobTypesService.GetJobTypes();
            var jobBenefits = await _jobBenefitsService.GetJobBenefits();
            jobVM.JobTypesList = jobTypes.Select(x => new SelectListItem { Text = x.Description, Value = x.Id.ToString() }).ToList();

            ViewBag.JobBenefits = jobBenefits.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description }).ToList();

            return View(jobVM).WithDanger("Error", "Some Errors Occurred");
        }

        // GET: Jobs/Delete/5
        [Authorize(Roles = "Admin,JobOwner")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _service.GetJobById(id.Value);
            if (job == null)
            {
                return NotFound();
            }

            var jobToDelete = new JobDeleteViewModel
            {
                Id = job.Id,
                Title = job.JobTitle
            };

            return View(jobToDelete);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,JobOwner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _service.Delete(id);

            return RedirectToAction(nameof(Index)).WithSuccess("", "Job deleted");
        }

        [HttpGet]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> JobApplication(Guid jobId)
        {
            var job = await _service.GetJobById(jobId);

            // Check if its already been applied for:
            var userid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobApplication = await _service.GetJobApplicationsForJob(jobId);
            if (jobApplication != null && jobApplication.Any(x => x.ApplicantId == userid))
            {
                // Already applied, pass back to Details view
                return RedirectToAction($"Details", new { id = jobId }).WithWarning("You were re-directed", "You have already applied for this job");
            }
            return View(job);
        }

        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JobApply(Guid jobId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.ApplyForJob(jobId, userId);
            if (result == null)
            {
                return View(result);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,JobOwner")]
        public async Task<IActionResult> ViewMyJobs(string sortOrder, string currentFilter, string searchString, bool showExpiredJobs, int? pageNumber, Guid? jobTypeId = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["MinSalarySortParm"] = sortOrder == "min_salary_asc" ? "min_salary_desc" : "min_salary_asc";
            ViewData["ClosingDateSortParm"] = sortOrder == "closing_date_asc" ? "closing_date_desc" : "closing_date_asc";
            ViewData["jobTypeId"] = jobTypeId;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobList = _service.GetMyJobs(userId, searchString, showExpiredJobs, jobTypeId);

            var jobTypes = await _jobTypesService.GetJobTypes();
            ViewData["JobTypes"] = jobTypes.OrderBy(x => x.Description).Where(x => x.IsActive).ToList();

            switch (sortOrder)
            {
                case "min_salary_desc":
                    jobList = jobList.OrderByDescending(x => x.MinSalary);
                    break;

                case "closing_date_desc":
                    jobList = jobList.OrderByDescending(x => x.ClosingDate);
                    break;

                case "closing_date_asc":
                    jobList = jobList.OrderBy(x => x.ClosingDate);
                    break;

                case "min_salary_asc":
                    jobList = jobList.OrderBy(x => x.MinSalary);
                    break;

                default:
                    break;
            }

            var jobDetailsViewModels = jobList.Select(job => new JobDetailsViewModel
            {
                Id = job.Id,
                JobTitle = job.JobTitle,
                Description = job.Description,
                MinSalary = job.MinSalary.GetValueOrDefault(),
                MaxSalary = job.MaxSalary.GetValueOrDefault(),
                ClosingDate = job.ClosingDate,
                PublishDate = job.PublishDate,
                CreatedDate = job.CreatedDate,
                IsActive = job.IsActive,
                JobType = job.JobType
            }).AsQueryable();

            int pageSize = 10;
            ViewData["totalPages"] = (jobList.Count() / pageSize) + 1;

            return View("MyJobs", await PaginatedList<JobDetailsViewModel>.CreateAsync(jobDetailsViewModels, pageNumber ?? 1, pageSize));
        }
    }
}