using System;
using System.Linq;
using System.Threading.Tasks;
using JobWebsiteMVC.Models;
using JobWebsiteMVC.Models.Job;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JobWebsiteMVC.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        public DataSeeder(ApplicationDbContext context, IServiceProvider service)
        {
            _context = context;
            var _userManager = service.GetRequiredService<UserManager<ApplicationUser>>();

            if (!_context.UserTypes.Any())
            {
                _context.AddRange(
                    new UserType { Description = "Job Seeker" },
                    new UserType { Description = "Job Owner" },
                    new UserType { Description = "Admin" }
                );
                _context.SaveChanges();
            }

            if (!_context.Users.Any())
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "test@test.com",
                    UserName = "TestUser",
                    DateOfBirth = new DateTime(1992, 4, 12),
                    FirstName = "Marcus",
                    LastName = "TestUser",
                    UserType = _context.UserTypes.FirstOrDefault(x => x.Description == "Job Owner")
                };
                _userManager.CreateAsync(user, "P@55w0rd1");

                ApplicationUser user2 = new ApplicationUser()
                {
                    Email = "test2@test.com",
                    UserName = "TestUser2",
                    DateOfBirth = new DateTime(1992, 11, 23),
                    FirstName = "Sarah",
                    LastName = "TestUser",
                    UserType = _context.UserTypes.FirstOrDefault(x => x.Description == "Job Seeker")
                };
                _userManager.CreateAsync(user2, "P@55w0rd1");

                ApplicationUser user3 = new ApplicationUser()
                {
                    Email = "admin@test.com",
                    UserName = "AdminTestUser2",
                    DateOfBirth = new DateTime(1987, 08, 14),
                    FirstName = "Admin",
                    LastName = "TestUser",
                    UserType = _context.UserTypes.FirstOrDefault(x => x.Description == "Admin")
                };
                _userManager.CreateAsync(user3, "P@55w0rd1");
            }
        }

        public async Task SeedDatabase()
        {
            var user1 = _context.Users.First();
            var user2 = _context.Users.Take(1).First();

            if (!_context.JobTypes.Any())
            {
                // Insert some job types:
                await _context.JobTypes.AddRangeAsync(
                    new JobType { Description = "Full Time", IsActive = true, CreatedBy = user1 },
                    new JobType { Description = "Part-Time", IsActive = true, CreatedBy = user1 },
                    new JobType { Description = "Contract", IsActive = true, CreatedBy = user1 },
                    new JobType { Description = "Permanent", IsActive = true, CreatedBy = user2 }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.JobBenefits.Any())
            {
                // Insert some job benefits:
                await _context.AddRangeAsync(
                    new JobBenefit { Description = "Cycle To Work Scheme", CreatedBy = user1, IsActive = true },
                    new JobBenefit { Description = "Pension", CreatedBy = user1, IsActive = true },
                    new JobBenefit { Description = "Work From Home", CreatedBy = user1, IsActive = true },
                    new JobBenefit { Description = "Flexi-Time", CreatedBy = user1, IsActive = true }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.Jobs.Any())
            {
                // Insert some jobs:
                await _context.AddRangeAsync(
                    new Job
                    {
                        JobTitle = "Demolition Worker",
                        Title = "Demolition Man",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        PublishDate = DateTime.Now,
                        WorkingHoursStart = new TimeSpan(08, 00, 00),
                        WorkingHoursEnd = new TimeSpan(17, 00, 00),
                        Description = "Demolition workers perform varied daily job duties depending on the demolition work that needs to be done, the size of the construction crew they work with, and the machinery and tools available to them. These core job tasks, however, are essentially the same everywhere.",
                        IsActive = true,
                        CreatedBy = user1,
                        MaxSalary = 12000M,
                        MinSalary = 10000M,
                        HolidayEntitlement = 21,
                        HoursPerWeek = 40,
                        JobType = _context.JobTypes.First()
                    },
                    new Job
                    {
                        JobTitle = "C# Software Devleloper",
                        Title = "C# Software Devleloper",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        CreatedDate = DateTime.Now,
                        CreatedBy = user1,
                        Description = "Generic software developer stuff",
                        HolidayEntitlement = 23,
                        HoursPerWeek = 37.5M,
                        IsActive = true,
                        JobType = _context.JobTypes.Take(1).First(),
                        MaxSalary = 40000M,
                        MinSalary = 38000,
                        PublishDate = DateTime.Now,
                        WorkingHoursStart = new TimeSpan(08, 30, 00),
                        WorkingHoursEnd = new TimeSpan(16, 30, 00)
                    },
                    new Job
                    {
                        JobTitle = "Journal Manager",
                        Title = "Journal Manager",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        CreatedDate = DateTime.Now,
                        CreatedBy = user1,
                        Description = "Manage the journals",
                        HolidayEntitlement = 30,
                        HoursPerWeek = 35,
                        IsActive = true,
                        JobType = _context.JobTypes.Take(1).First(),
                        MaxSalary = 28000M,
                        MinSalary = 24000,
                        PublishDate = DateTime.Now,
                        WorkingHoursStart = new TimeSpan(09, 00, 00),
                        WorkingHoursEnd = new TimeSpan(16, 30, 00)
                    },
                    new Job
                    {
                        JobTitle = "Office Secretary",
                        Title = "Office Secretary",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        CreatedDate = DateTime.Now,
                        CreatedBy = user1,
                        Description = "Usual secretarial duties",
                        HolidayEntitlement = 12,
                        HoursPerWeek = 60,
                        IsActive = true,
                        JobType = _context.JobTypes.Take(1).First(),
                        MaxSalary = 20000M,
                        MinSalary = 18000,
                        PublishDate = DateTime.Now,
                        WorkingHoursStart = new TimeSpan(07, 00, 00),
                        WorkingHoursEnd = new TimeSpan(18, 30, 00)
                    },
                    new Job
                    {
                        JobTitle = "LGV Driver",
                        Title = "LGV Driver",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        CreatedDate = DateTime.Now,
                        CreatedBy = user1,
                        Description = "Drive big trucks to places, unload the required stock, rinse and repeat",
                        HolidayEntitlement = 31,
                        HoursPerWeek = 50,
                        IsActive = true,
                        JobType = _context.JobTypes.Take(1).First(),
                        MaxSalary = 32000M,
                        MinSalary = 24000,
                        PublishDate = DateTime.Now,
                        WorkingHoursStart = new TimeSpan(05, 00, 00),
                        WorkingHoursEnd = new TimeSpan(15, 30, 00)
                    },
                    new Job
                    {
                        JobTitle = "Fudge Packer",
                        Title = "Fudge Packer",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        CreatedDate = DateTime.Now,
                        CreatedBy = user1,
                        Description = "To pack as much fudge as possible",
                        HolidayEntitlement = 20,
                        HoursPerWeek = 20,
                        IsActive = true,
                        JobType = _context.JobTypes.Take(1).First(),
                        MaxSalary = 22000M,
                        MinSalary = 2000,
                        PublishDate = DateTime.Now,
                        WorkingHoursStart = new TimeSpan(09, 00, 00),
                        WorkingHoursEnd = new TimeSpan(15, 30, 00)
                    }
                );
                await _context.SaveChangesAsync();
            }
        }
    }
}