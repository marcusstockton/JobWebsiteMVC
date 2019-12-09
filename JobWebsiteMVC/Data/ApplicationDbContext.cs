using JobWebsiteMVC.Models;
using JobWebsiteMVC.Models.Job;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobWebsiteMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobBenefit> JobBenefits { get; set; }
        public DbSet<Job_JobBenefit> Job_JobBenefits { get; set; }
        public DbSet<JobType> JobTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Job>().HasKey(x => x.Id);

            builder.Entity<Job>()
                .HasOne<JobType>()
                .WithMany();

            builder.Entity<Job_JobBenefit>().HasKey(bc => new { bc.JobId, bc.JobBenefitId });

            builder.Entity<Job_JobBenefit>()
                    .HasOne<Job>(bc => bc.Job)
                    .WithMany(b => b.Job_JobBenefits)
                    .HasForeignKey(bc => bc.JobId);

            builder.Entity<Job_JobBenefit>()
                    .HasOne<JobBenefit>(bc => bc.JobBenefit)
                    .WithMany(c => c.Job_JobBenefits)
                    .HasForeignKey(bc => bc.JobBenefitId);
        }

        public DbSet<JobWebsiteMVC.Models.Job.JobType> JobType { get; set; }
    }
}