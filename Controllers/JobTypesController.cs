using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Models.Job;

namespace JobWebsiteMVC.Controllers
{
    public class JobTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobType.ToListAsync());
        }

        // GET: JobTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _context.JobType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobType == null)
            {
                return NotFound();
            }

            return View(jobType);
        }

        // GET: JobTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Id,CreatedDate,UpdatedDate,IsActive")] JobType jobType)
        {
            if (ModelState.IsValid)
            {
                jobType.Id = Guid.NewGuid();
                jobType.CreatedDate = DateTime.Now;
                _context.Add(jobType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobType);
        }

        // GET: JobTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _context.JobType.FindAsync(id);
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
                    jobType.UpdatedDate = DateTime.Now;
                    _context.Update(jobType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobTypeExists(jobType.Id))
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
            return View(jobType);
        }

        // GET: JobTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _context.JobType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobType == null)
            {
                return NotFound();
            }

            return View(jobType);
        }

        // POST: JobTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var jobType = await _context.JobType.FindAsync(id);
            _context.JobType.Remove(jobType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobTypeExists(Guid id)
        {
            return _context.JobType.Any(e => e.Id == id);
        }
    }
}
