using System;
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

namespace JobWebsiteMVC.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public JobsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var jobList = await _context.Jobs
                .Include(x=>x.JobType)
                .ToListAsync();

            var jobs = _mapper.Map<List<JobDetailsViewModel>>(jobList);

            return View(jobs);
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(x => x.Job_JobBenefits)
                .ThenInclude(x=>x.JobBenefit)
                .Include(x=>x.JobType)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            var jobVM = _mapper.Map<JobDetailsViewModel>(job);
            return View(jobVM);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            var job = new JobCreateViewModel();
            ViewBag.JobBenefits = _context.JobBenefits.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description}).ToList();
            job.JobTypesList =  _context.JobTypes.AsNoTracking()
                    .OrderBy(n => n.Description)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.Id.ToString(),
                            Text = n.Description
                        }).ToList();
                        
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
                job.Id = Guid.NewGuid();
                job.CreatedDate = DateTime.Now;

                var jjb = new List<Job_JobBenefit>();
                foreach (var item in jobVM.JobBenefitsIds)
                {
                    jjb.Add(new Job_JobBenefit { JobId = job.Id, JobBenefitId = item });
                }
                job.Job_JobBenefits = jjb;
                await _context.AddRangeAsync(jjb);
                await _context.AddAsync(job);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobVM);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var job = await _context.Jobs
                .Include(x => x.Job_JobBenefits)
                .ThenInclude(x=>x.JobBenefit)
                .Include(x=>x.JobType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            var jobVM = _mapper.Map<JobEditViewModel>(job);
            jobVM.JobBenefitsIds = jobVM.Job_JobBenefits.Select(s=>s.JobBenefitId).ToList();
            jobVM.JobTypesList = _context.JobTypes.Select(x=> new SelectListItem{Text = x.Description, Value = x.Id.ToString()}).ToList();

            ViewBag.JobBenefits = _context.JobBenefits.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description}).ToList();
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
                    var jobToUpdate = await _context.Jobs
                                    .Include(x => x.Job_JobBenefits)
                                    .ThenInclude(x=>x.JobBenefit)
                                    .FirstOrDefaultAsync(m => m.Id == id);

                    if(await TryUpdateModelAsync<Job>(jobToUpdate, "", 
                        x=>x.MaxSalary, 
                        x=>x.MinSalary, 
                        x=>x.PublishDate, 
                        x=>x.Title, 
                        x=>x.WorkingHoursEnd, 
                        x=>x.WorkingHoursStart, 
                        x=>x.Description, 
                        x=>x.HolidayEntitlement,
                        x=>x.HoursPerWeek,
                        x=>x.IsActive,
                        x=>x.IsDraft,
                        x=>x.JobTypeId,
                        x=>x.ClosingDate))
                    {
                        jobToUpdate.UpdatedDate = DateTime.Now;
                        UpdateJobBenefits(jobVM.JobBenefitsIds, jobToUpdate);

                        _context.Update(jobToUpdate);
                        await _context.SaveChangesAsync();
                    }
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
                return RedirectToAction(nameof(Index));
            }
            return View(jobVM);
        }

        private void UpdateJobBenefits(List<Guid> jobBenefitsIds, Job jobToUpdate)
        {
            var tagLinksToDelete =
                _context.Job_JobBenefits.Where(x => !jobBenefitsIds.Contains(x.JobBenefitId) && x.JobId == jobToUpdate.Id).ToList();

            var tagLinksToAdd = jobBenefitsIds
                .Where(x => !_context.Job_JobBenefits.Any(y => y.JobBenefitId == x && y.JobId == jobToUpdate.Id))
                .Select(z => new Job_JobBenefit {JobId = jobToUpdate.Id, JobBenefitId = z}).ToList();

            tagLinksToDelete.ForEach(x => _context.Job_JobBenefits.Remove(x));
            tagLinksToAdd.ForEach(x => _context.Job_JobBenefits.Add(x));
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(Guid id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
