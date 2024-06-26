﻿using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Areas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        // GET: api/Jobs
        [HttpGet, Route("GetMyJobs")]
        public async Task<ActionResult<IEnumerable<Job>>> GetMyJobs()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _jobService.GetMyJobs(userId, null, false, null)
                .OrderByDescending(x => x.CreatedDate)
                .ThenByDescending(x => x.UpdatedDate)
                .ToListAsync();
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(Guid id)
        {
            var job = await _jobService.GetJobById(id);

            if (job == null)
            {
                return NotFound();
            }

            return job;
        }

        //// PUT: api/Jobs/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutJob(Guid id, Job job)
        //{
        //    if (id != job.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(job).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!JobExists(id))
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

        //// POST: api/Jobs
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Job>> PostJob(Job job)
        //{
        //    _context.Jobs.Add(job);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetJob", new { id = job.Id }, job);
        //}

        //// DELETE: api/Jobs/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteJob(Guid id)
        //{
        //    var job = await _context.Jobs.FindAsync(id);
        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Jobs.Remove(job);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool JobExists(Guid id)
        //{
        //    return _context.Jobs.Any(e => e.Id == id);
        //}
    }
}