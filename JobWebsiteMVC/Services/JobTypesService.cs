using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using Microsoft.EntityFrameworkCore;

namespace JobWebsiteMVC.Services
{
    public class JobTypesService : IJobTypesService
    {
        private ApplicationDbContext _context;
        public JobTypesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobType>> GetJobTypes()
        {
            return await _context.JobTypes.ToListAsync();
        }
    }
}
