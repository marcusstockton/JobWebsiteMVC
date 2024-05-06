using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace JobWebsiteMVC.Data
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        public JobRepository(ApplicationDbContext context, ILogger logger) : base(context, logger) { }

        public override async Task<Job> GetById(Guid id)
        {
            return await context.Jobs
                .Include(x => x.JobBenefits)
                    .ThenInclude(x => x.Benefit)
                .Include(x => x.JobType)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
