using JobWebsiteMVC.Models;
using JobWebsiteMVC.Models.Job;
using JobWebsiteMVC.ViewModels.Job;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Reflection.Emit;

namespace JobWebsiteMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<JobBenefit> JobBenefits { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Job>()
                .HasKey(x => x.Id);

            //builder.Entity<Job>().Property(x => x.MinSalary).HasConversion<double>(); // for sorting...

            builder.Entity<Job>()
                .HasOne<JobType>()
                .WithMany();

            builder.Entity<Job>()
                .HasOne<JobType>()
                .WithMany();

            builder.Entity<ApplicationUser>()
                .HasMany(c => c.Attachments)
                .WithOne(e => e.User)
                .HasForeignKey(x => x.UserId);

            builder.Entity<JobBenefit>().HasKey(bc => new { bc.JobId, bc.JobBenefitId });
            builder.Entity<JobBenefit>()
                    .HasOne<Job>(bc => bc.Job)
                    .WithMany(b => b.JobBenefits)
                    .HasForeignKey(bc => bc.JobId);
            builder.Entity<JobBenefit>()
                    .HasOne<Benefit>(bc => bc.Benefit)
                    .WithMany(c => c.Job_JobBenefits)
                    .HasForeignKey(bc => bc.JobBenefitId);

            // Properties:
            builder.Entity<JobType>().Property(x => x.Description).IsRequired().HasMaxLength(100);
            builder.Entity<Benefit>().Property(x => x.Description).IsRequired().HasMaxLength(100);


            // Do some conversions to handle how lame SqlLite is..
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var dateTimeOffsetProperties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                            || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in dateTimeOffsetProperties)
                {
                    builder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter()); // The converter!
                }
                // convert all decimals to doubles for sqlite
                var decimalProperties = entityType.ClrType.GetProperties().Where(x => x.PropertyType == typeof(decimal) 
                                                                                || x.PropertyType == typeof(decimal?));
                foreach (var property in decimalProperties)
                {
                    builder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion<double>();
                }
            }
        }

        public DbSet<JobDetailsViewModel> JobDetailsViewModel { get; set; }
    }
}