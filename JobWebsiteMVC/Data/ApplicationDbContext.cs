using JobWebsiteMVC.Models;
using JobWebsiteMVC.Models.Job;
using JobWebsiteMVC.ViewModels.Job;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<JobDetailsViewModel> JobDetailsViewModel { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Job>()
                .HasKey(x => x.Id);

            builder.Entity<Job>()
                .HasOne(x => x.JobType)
                .WithMany()
                .HasForeignKey(x => x.JobTypeId);

            builder.Entity<JobTitle>()
                .HasKey(x => x.Id);

            builder.Entity<JobTitle>()
                .HasIndex(x => x.Description)
                .IsUnique();
            builder.Entity<JobTitle>()
                .Property(x => x.IsActive).HasDefaultValue(true);

            builder.Entity<ApplicationUser>()
                .HasMany(c => c.Attachments)
                .WithOne(e => e.User)
                .HasForeignKey(x => x.UserId);

            builder.Entity<JobBenefit>().HasKey(bc => new { bc.JobId, bc.JobBenefitId });
            builder.Entity<JobBenefit>()
                    .HasOne(bc => bc.Job)
                    .WithMany(b => b.JobBenefits)
                    .HasForeignKey(bc => bc.JobId);
            builder.Entity<JobBenefit>()
                    .HasOne(bc => bc.Benefit)
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

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                        .Entries()
                        .Where(e => e.Entity is Base && (
                                e.State == EntityState.Added
                                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Modified)
                {
                    ((Base)entityEntry.Entity).UpdatedDate = DateTime.Now;
                }
                if (entityEntry.State == EntityState.Added)
                {
                    ((Base)entityEntry.Entity).CreatedDate = DateTime.Now;
                    ((Base)entityEntry.Entity).UpdatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                        .Entries()
                        .Where(e => e.Entity is Base && (
                                e.State == EntityState.Added
                                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Modified)
                {
                    ((Base)entityEntry.Entity).UpdatedDate = DateTime.Now;
                }

                if (entityEntry.State == EntityState.Added)
                {
                    ((Base)entityEntry.Entity).CreatedDate = DateTime.Now;
                    ((Base)entityEntry.Entity).UpdatedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}