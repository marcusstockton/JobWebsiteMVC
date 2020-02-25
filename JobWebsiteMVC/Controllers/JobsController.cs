﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Models.Job;
using JobWebsiteMVC.ViewModels.Job;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using JobWebsiteMVC.Extensions.Alerts;
using JobWebsiteMVC.Interfaces;

namespace JobWebsiteMVC.Controllers
{
    public class JobsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<JobsController> _logger;
        private readonly IJobService _service;
        private readonly IJobTypesService _jobTypesService;
        private readonly IJobBenefitsService _jobBenefitsService;

        public JobsController(IMapper mapper, ILogger<JobsController> logger, IJobService service, IJobTypesService jobTypesService, IJobBenefitsService jobBenefitsService)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
            _jobTypesService = jobTypesService;
            _jobBenefitsService = jobBenefitsService;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var jobList = await _service.GetJobs();

            var jobs = _mapper.Map<List<JobDetailsViewModel>>(jobList);
            _logger.LogInformation($"Found {jobList.Count} jobs");
            return View(jobs);
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var job = await _context.Jobs
            //     .Include(x => x.Job_JobBenefits)
            //         .ThenInclude(x=>x.JobBenefit)
            //     .Include(x=>x.JobType)
            //     .AsNoTracking()
            //     .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create(JobCreateViewModel jobVM) // [Bind("Title,Description,IsDraft,MinSalary,MaxSalary,WorkingHoursStart,WorkingHoursEnd,HoursPerWeek,HolidayEntitlement,ClosingDate,PublishDate,Id,CreatedDate,UpdatedDate,IsActive")]
        {
            if (ModelState.IsValid)
            {
               var job = _mapper.Map<Job>(jobVM);
               await _service.Post(job);
               _jobBenefitsService.CreateOrUpdateJobBenefitsForJob( job.Id, new List<Job_JobBenefit>(), jobVM.JobBenefitsIds );

                return RedirectToAction(nameof(Index)).WithSuccess("Success", "Job sucessfully created!");
            }
            return View(jobVM).WithDanger("Error", "Some errors occured creating the job");
        }

        // GET: Jobs/Edit/5
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
            jobVM.JobBenefitsIds = jobVM.Job_JobBenefits.Select(s=>s.JobBenefitId).ToList();
            jobVM.JobTypesList = jobTypes.Select(x=> new SelectListItem{Text = x.Description, Value = x.Id.ToString()}).ToList();

            ViewBag.JobBenefits = jobBenefits.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description}).ToList();
            return View(jobVM);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description,IsDraft,MinSalary,MaxSalary,WorkingHoursStart,WorkingHoursEnd,HoursPerWeek,HolidayEntitlement,ClosingDate,PublishDate,Id,CreatedDate,UpdatedDate,IsActive,JobBenefitsIds,JobTypeId")] JobEditViewModel jobVM)
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
                    _jobBenefitsService.CreateOrUpdateJobBenefitsForJob(job.Id, currentJobBenefits, jobVM.JobBenefitsIds);

                    await _service.Put(job);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(jobVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)).WithSuccess("Success","Job Updated Sucessfully!");
            }
            return View(jobVM).WithDanger("Error", "Some Errors Occured");
        }

        
        // GET: Jobs/Delete/5
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _service.Delete(id);
         
            return RedirectToAction(nameof(Index)).WithSuccess("", "Job deleted");
        }

        [HttpGet]
        public async Task<IActionResult> JobApplication(Guid jobId)
        {
            var job = await _service.GetJobById(jobId);

            // Check if its already been applied for:
            var userid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobApplication = await _service.GetJobApplicationsForJob( jobId );
            if (jobApplication != null && jobApplication.Any(x=>x.ApplicantId == userid))
            {
                // Already applied, pass back to Details view
                return RedirectToAction( $"Details", new { id = jobId } ).WithWarning( "You were re-directed", "You have already applied for this job" );
            }
            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JobApply(Guid jobId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.ApplyForJob( jobId, userId );
            if (result == null)
            {
                return View( result );
            }
            return RedirectToAction("Index");
        }

        private bool JobExists(Guid id)
        {
            //return _context.Jobs.Any(e => e.Id == id);
            return false;
        }
    }
}
