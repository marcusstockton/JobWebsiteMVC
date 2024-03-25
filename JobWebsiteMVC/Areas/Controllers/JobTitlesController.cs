using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Models.Job;

namespace JobWebsiteMVC.Areas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitlesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobTitlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTitle>>> GetJobTitles()
        {
            return await _context.JobTitles.Where(x => x.IsActive).ToListAsync();
        }

        [HttpGet("{description}")]
        public async Task<ActionResult<List<JobTitle>>> GetJobTitle(string description)
        {
            return Ok(await _context.JobTitles
                .Where(x => x.Description.StartsWith(description))
                .Where(x=>x.IsActive)
                .Take(100)
                .ToListAsync());
        }

        //// PUT: api/JobTitles/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutJobTitle(int id, JobTitle jobTitle)
        //{
        //    if (id != jobTitle.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(jobTitle).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!JobTitleExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/JobTitles
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<JobTitle>> PostJobTitle(JobTitle jobTitle)
        //{
        //    _context.JobTitles.Add(jobTitle);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetJobTitle", new { id = jobTitle.Id }, jobTitle);
        //}

        //// DELETE: api/JobTitles/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteJobTitle(int id)
        //{
        //    var jobTitle = await _context.JobTitles.FindAsync(id);
        //    if (jobTitle == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.JobTitles.Remove(jobTitle);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool JobTitleExists(int id)
        //{
        //    return _context.JobTitles.Any(e => e.Id == id);
        //}
    }
}
