using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Models.Job;

namespace JobWebsiteMVC.Controllers
{
    public class JobBenefitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobBenefitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobBenefits
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobBenefits.ToListAsync());
        }

        // GET: JobBenefits/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobBenefit = await _context.JobBenefits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobBenefit == null)
            {
                return NotFound();
            }

            return View(jobBenefit);
        }

        // GET: JobBenefits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobBenefits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Id,CreatedDate,UpdatedDate,IsActive")] JobBenefit jobBenefit)
        {
            if (ModelState.IsValid)
            {
                jobBenefit.Id = Guid.NewGuid();
                jobBenefit.CreatedDate = DateTime.Now;
                _context.Add(jobBenefit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobBenefit);
        }

        // GET: JobBenefits/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobBenefit = await _context.JobBenefits.FindAsync(id);
            if (jobBenefit == null)
            {
                return NotFound();
            }
            return View(jobBenefit);
        }

        // POST: JobBenefits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,Id,CreatedDate,UpdatedDate,IsActive")] JobBenefit jobBenefit)
        {
            if (id != jobBenefit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    jobBenefit.UpdatedDate = DateTime.Now;
                    _context.Update(jobBenefit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobBenefitExists(jobBenefit.Id))
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
            return View(jobBenefit);
        }

        // GET: JobBenefits/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobBenefit = await _context.JobBenefits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobBenefit == null)
            {
                return NotFound();
            }

            return View(jobBenefit);
        }

        // POST: JobBenefits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var jobBenefit = await _context.JobBenefits.FindAsync(id);
            _context.JobBenefits.Remove(jobBenefit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobBenefitExists(Guid id)
        {
            return _context.JobBenefits.Any(e => e.Id == id);
        }
    }
}
