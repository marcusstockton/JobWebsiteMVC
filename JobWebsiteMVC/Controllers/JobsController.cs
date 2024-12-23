﻿using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ILogger<JobsController> _logger;
        private readonly IJobService _service;
        private readonly IJobTypesService _jobTypesService;
        private readonly IJobBenefitsService _jobBenefitsService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IHubContext<NotificationsHub, INotificationsHub> _hubContext;

        public JobsController(IMapper mapper, ILogger<JobsController> logger, IJobService service, IJobTypesService jobTypesService, 
            IJobBenefitsService jobBenefitsService, IUnitOfWork unitOfWork, IHubContext<NotificationsHub, INotificationsHub> hubContext)
        {
            _mapper = mapper;
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
            //ViewData["ClosingDateSortParm"] = sortOrder == "closing_date_asc" ? "closing_date_desc" : "closing_date_asc";
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

            var jobList = _service.GetJobs(searchString, showExpiredJobs, jobTypeId).OrderByDescending(x=>x.CreatedDate);

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

                default:
                    break;
            }
            var collection = jobList.ProjectTo<JobListViewModel>(_mapper.ConfigurationProvider).AsQueryable();
            int pageSize = 10;
            ViewData["totalPages"] = (jobList.Count() / pageSize) + 1;

            return View(await PaginatedList<JobListViewModel>.CreateAsync(collection, pageNumber ?? 1, pageSize));
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

            var jobVM = _mapper.Map<JobDetailsViewModel>(job);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,JobOwner")]
        public async Task<IActionResult> Create(JobCreateViewModel jobVM) // [Bind("Title,Description,IsDraft,MinSalary,MaxSalary,WorkingHoursStart,WorkingHoursEnd,HoursPerWeek,HolidayEntitlement,ClosingDate,PublishDate,Id,CreatedDate,UpdatedDate,IsActive")]
        {
            if (ModelState.IsValid)
            {
                var job = _mapper.Map<Job>(jobVM);
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _service.Post(job, userid);
                await _jobBenefitsService.CreateOrUpdateJobBenefitsForJob(job.Id, new List<JobBenefit>(), jobVM.JobBenefitsIds);

                return RedirectToAction(nameof(Index)).WithSuccess("Success", "Job sucessfully created!");
            }
            return View(jobVM).WithDanger("Error", "Some errors occured creating the job");
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

            var jobVM = _mapper.Map<JobEditViewModel>(job);
            var jobTypes = await _jobTypesService.GetJobTypes();
            var jobBenefits = await _jobBenefitsService.GetJobBenefits();
            jobVM.JobBenefitsIds = jobVM.Job_JobBenefits?.Select(s => s.JobBenefitId).ToList();
            jobVM.JobTypesList = jobTypes.Select(x => new SelectListItem { Text = x.Description, Value = x.Id.ToString() }).ToList();

            ViewBag.JobBenefits = jobBenefits.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description }).ToList();
            return View(jobVM);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    var job = _mapper.Map<Job>(jobVM);
                    var currentJobBenefits = await _jobBenefitsService.GetJobBenefitsForJobId(job.Id);
                    // Update JobBenefits:
                    await _jobBenefitsService.CreateOrUpdateJobBenefitsForJob(job.Id, currentJobBenefits, jobVM.JobBenefitsIds);

                    await _service.Put(job);

                    await _hubContext.Clients.All.SendMessage(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString(), "This is a Test");

                    return RedirectToAction(nameof(Index)).WithSuccess("Success", "Job Updated Sucessfully!");
                }
                catch (Exception ex)
                {
                    return View(jobVM).WithDanger("Error", ex.Message);
                }
            }

            var jobTypes = await _jobTypesService.GetJobTypes();
            var jobBenefits = await _jobBenefitsService.GetJobBenefits();
            jobVM.JobBenefitsIds = jobVM.Job_JobBenefits?.Select(s => s.JobBenefitId).ToList();
            jobVM.JobTypesList = jobTypes.Select(x => new SelectListItem { Text = x.Description, Value = x.Id.ToString() }).ToList();

            ViewBag.JobBenefits = jobBenefits.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description }).ToList();

            return View(jobVM).WithDanger("Error", "Some Errors Occured");
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
            var jobToDelete = _mapper.Map<JobDeleteViewModel>(job);
            if (job == null)
            {
                return NotFound();
            }

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

            var collection = jobList.ProjectTo<JobDetailsViewModel>(_mapper.ConfigurationProvider);

            int pageSize = 10;
            ViewData["totalPages"] = (jobList.Count() / pageSize) + 1;

            return View("MyJobs", await PaginatedList<JobDetailsViewModel>.CreateAsync(collection, pageNumber ?? 1, pageSize));

            //var jobs = _mapper.Map<List<JobDetailsViewModel>>(result);
            //return View("MyJobs", jobs);
        }
    }
}