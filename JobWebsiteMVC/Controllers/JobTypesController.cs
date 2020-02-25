using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JobWebsiteMVC.Models.Job;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Extensions.Alerts;
using Microsoft.AspNetCore.Authorization;

namespace JobWebsiteMVC.Controllers
{
    public class JobTypesController : Controller
    {
        private readonly IJobTypesService _jobTypeService;

        public JobTypesController(IJobTypesService jobTypeService)
        {
            _jobTypeService = jobTypeService;
        }

        // GET: JobTypes
        public async Task<IActionResult> Index()
        {
            return View(await _jobTypeService.GetJobTypes());
        }

        // GET: JobTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _jobTypeService.GetJobTypeById(id.Value);
            if (jobType == null)
            {
                return NotFound();
            }

            return View(jobType);
        }

        // GET: JobTypes/Create
        [Authorize(Policy = "JobOwnerOnly")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "JobOwnerOnly")]
        public async Task<IActionResult> Create([Bind("Description,Id,CreatedDate,UpdatedDate,IsActive")] JobType jobType)
        {
            if (ModelState.IsValid)
            {

                jobType.CreatedDate = DateTime.Now;
                await _jobTypeService.CreateJobType(jobType);
                return RedirectToAction(nameof(Index));
            }
            var query = ModelState.Values.SelectMany(v => v.Errors);
            var errorList = query.ToList();
            return View(jobType).WithDanger("Error", errorList.ToString());
        }

        // GET: JobTypes/Edit/5
        [Authorize(Policy = "JobOwnerOnly")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _jobTypeService.GetJobTypeById(id.Value);
            if (jobType == null)
            {
                return NotFound();
            }
            return View(jobType);
        }

        // POST: JobTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "JobOwnerOnly")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,Id,CreatedDate,UpdatedDate,IsActive")] JobType jobType)
        {
            if (id != jobType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _jobTypeService.UpdateJobType(jobType);
                }
                catch (Exception ex)
                {
                    return View(jobType).WithDanger("Error", ex.Message);
                }
                return RedirectToAction(nameof(Index)).WithSuccess("Success", "Job type sucessfully updated!");
            }
            var query = ModelState.Values.SelectMany(v => v.Errors);
            var errorList = query.ToList();

            return View(jobType).WithDanger("Error", errorList.ToString());
        }

        // GET: JobTypes/Delete/5
        [Authorize(Policy = "JobOwnerOnly")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _jobTypeService.GetJobTypeById(id.Value);
            if (jobType == null)
            {
                return NotFound();
            }

            return View(jobType);
        }

        // POST: JobTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "JobOwnerOnly")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var jobType = await _jobTypeService.GetJobTypeById(id);
            if(jobType != null)
            {
                await _jobTypeService.DeleteJobType(jobType);
                return RedirectToAction(nameof(Index)).WithSuccess("Success", "Job Type deleted");
            }
            return View(jobType);
        }
    }
}
