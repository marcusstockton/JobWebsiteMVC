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
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobBenefit> JobBenefits { get; set; }
        public DbSet<Job_JobBenefit> Job_JobBenefits { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<Job_JobSkill> Job_JobSkills { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserType>()
                .HasMany<ApplicationUser>()
                .WithOne(x=>x.UserType)
                .HasForeignKey(x=>x.UserTypeId);

            builder.Entity<Job>()
                .HasKey(x => x.Id);

            builder.Entity<Job>()
                .HasOne<JobType>()
                .WithMany();

            builder.Entity<ApplicationUser>()
                .HasMany(c => c.Attachments)
                .WithOne(e => e.User)
                .HasForeignKey(x=>x.UserId);

            builder.Entity<Job_JobBenefit>().HasKey(bc => new { bc.JobId, bc.JobBenefitId });
            builder.Entity<Job_JobBenefit>()
                    .HasOne<Job>(bc => bc.Job)
                    .WithMany(b => b.Job_JobBenefits)
                    .HasForeignKey(bc => bc.JobId);
            builder.Entity<Job_JobBenefit>()
                    .HasOne<JobBenefit>(bc => bc.JobBenefit)
                    .WithMany(c => c.Job_JobBenefits)
                    .HasForeignKey(bc => bc.JobBenefitId);

            builder.Entity<Job_JobSkill>().HasKey(bc => new { bc.JobId, bc.JobSkillId });
            builder.Entity<Job_JobSkill>()
                    .HasOne<Job>(bc => bc.Job)
                    .WithMany(b => b.Job_JobSkills)
                    .HasForeignKey(bc => bc.JobId);
            builder.Entity<Job_JobSkill>()
                    .HasOne<JobSkill>(bc => bc.JobSkill)
                    .WithMany(c => c.Job_JobSkills)
                    .HasForeignKey(bc => bc.JobSkillId);
        }
    }
}
