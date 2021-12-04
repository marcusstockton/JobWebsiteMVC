using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobWebsiteMVC.Models;
using JobWebsiteMVC.Models.Job;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JobWebsiteMVC.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(ApplicationDbContext context, IServiceProvider service)
        {
            _context = context;
            _userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        }

        public async Task SeedDatabase(bool reseedDatabase = false)
        {
            if (reseedDatabase)
            {
                _context.Database.EnsureDeleted();
                _context.Database.Migrate();
            }

            if (!_context.Roles.Any())
            {
                string[] roleNames = { "Admin", "JobSeeker", "JobOwner" };
                foreach (var roleName in roleNames)
                {
                    var roleExist = _roleManager.RoleExistsAsync(roleName).Result;
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }

            if (!_context.Users.Any())
            {
                ApplicationUser jobOwner1 = new ApplicationUser()
                {
                    Email = "test@test.com",
                    UserName = "TestUser",
                    DateOfBirth = new DateTime(1992, 4, 12),
                    FirstName = "Marcus",
                    LastName = "TestUser",
                };
                await _userManager.CreateAsync(jobOwner1, "P@55w0rd1");
                await _userManager.AddToRoleAsync(jobOwner1, "JobOwner");

                ApplicationUser jobSeeker1 = new ApplicationUser()
                {
                    Email = "test2@test.com",
                    UserName = "TestUser2",
                    DateOfBirth = new DateTime(1992, 11, 23),
                    FirstName = "Sarah",
                    LastName = "TestUser",
                };
                await _userManager.CreateAsync(jobSeeker1, "P@55w0rd1");
                await _userManager.AddToRoleAsync(jobSeeker1, "JobSeeker");

                ApplicationUser jobSeeker2 = new ApplicationUser()
                {
                    Email = "jobSeeker2@test.com",
                    UserName = "JobSeeker2",
                    DateOfBirth = new DateTime(1987, 01, 14),
                    FirstName = "Dave",
                    LastName = "Richmond",
                };
                await _userManager.CreateAsync(jobSeeker2, "P@55w0rd1");
                await _userManager.AddToRoleAsync(jobSeeker2, "JobSeeker");

                ApplicationUser adminUser1 = new ApplicationUser()
                {
                    Email = "admin@test.com",
                    UserName = "AdminTestUser2",
                    DateOfBirth = new DateTime(1987, 08, 14),
                    FirstName = "Admin",
                    LastName = "TestUser",
                };
                await _userManager.CreateAsync(adminUser1, "P@55w0rd1");
                await _userManager.AddToRoleAsync(adminUser1, "Admin");
            }

            var jobOwner = await _context.Users.FindAsync("TestUser");
            var adminUser = await _context.Users.FindAsync("AdminTestUser2");
            var jobSeeker = await _context.Users.FindAsync("TestUser2");

            if (!_context.Attachments.Any())
            {
                await _context.Attachments.AddAsync(new Attachment
                {
                    CreatedBy = jobSeeker,
                    CreatedDate = DateTime.Now,
                    FileName = "examplePic.jpg",
                    Location = "~/Uploads/Example/examplePic.jpg",
                    FileType = "jpg",
                    IsActive = true,
                    User = jobSeeker,
                });
                await _context.SaveChangesAsync();
            }

            if (!_context.JobTypes.Any())
            {
                // Insert some job types:
                await _context.JobTypes.AddRangeAsync(
                    new JobType { Description = "Full Time", IsActive = true, CreatedDate = DateTime.Now, CreatedBy = jobOwner },
                    new JobType { Description = "Part-Time", IsActive = true, CreatedDate = DateTime.Now, CreatedBy = jobOwner },
                    new JobType { Description = "Contract", IsActive = true, CreatedDate = DateTime.Now, CreatedBy = jobOwner },
                    new JobType { Description = "Permanent", IsActive = true, CreatedDate = DateTime.Now, CreatedBy = adminUser }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.JobBenefits.Any())
            {
                // Insert some job benefits:
                await _context.AddRangeAsync(
                    new JobBenefit { Description = "Cycle To Work Scheme", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true },
                    new JobBenefit { Description = "Pension", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true },
                    new JobBenefit { Description = "Work From Home", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true },
                    new JobBenefit { Description = "Flexi-Time", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true },
                    new JobBenefit { Description = "Health insurance", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true },
                    new JobBenefit { Description = "Childcare benefits", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true },
                    new JobBenefit { Description = "Relocation assistance", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true },
                    new JobBenefit { Description = "Gym Membership", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true },
                    new JobBenefit { Description = "Critical Illness Cover", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true },
                    new JobBenefit { Description = "Death in Service Cover", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true },
                    new JobBenefit { Description = "Private Medical Insurance", CreatedBy = adminUser, CreatedDate = DateTime.Now, IsActive = true }
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
                        CreatedDate = DateTime.Now.AddDays(-1),
                        PublishDate = DateTime.Now,
                        WorkingHoursStart = new TimeSpan(08, 00, 00),
                        WorkingHoursEnd = new TimeSpan(17, 00, 00),
                        Description = "Demolition workers perform varied daily job duties depending on the demolition work that needs to be done, the size of the construction crew they work with, and the machinery and tools available to them. These core job tasks, however, are essentially the same everywhere.",
                        IsActive = true,
                        CreatedBy = jobOwner,
                        MaxSalary = 12000M,
                        MinSalary = 10000M,
                        HolidayEntitlement = 21,
                        HoursPerWeek = 40,
                        JobType = _context.JobTypes.First(),
                        Job_JobBenefits = _context.Job_JobBenefits.Take(2).ToList(),
                    },
                    new Job
                    {
                        JobTitle = "C# Software Devleloper",
                        Title = "C# Software Devleloper",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        CreatedDate = DateTime.Now,
                        CreatedBy = jobOwner,
                        Description = "Generic software developer stuff",
                        HolidayEntitlement = 23,
                        HoursPerWeek = 37.5M,
                        IsActive = true,
                        JobType = _context.JobTypes.Take(2).First(),
                        MaxSalary = 40000M,
                        MinSalary = 38000,
                        PublishDate = DateTime.Now,
                        WorkingHoursStart = new TimeSpan(08, 30, 00),
                        WorkingHoursEnd = new TimeSpan(16, 30, 00),
                        Job_JobBenefits = _context.Job_JobBenefits.Skip(1).Take(2).ToList(),
                    },
                    new Job
                    {
                        JobTitle = "Journal Manager",
                        Title = "Journal Manager",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        CreatedDate = DateTime.Now,
                        CreatedBy = jobOwner,
                        Description = "Manage the journals",
                        HolidayEntitlement = 30,
                        HoursPerWeek = 35,
                        IsActive = true,
                        JobType = _context.JobTypes.First(),
                        MaxSalary = 28000M,
                        MinSalary = 24000,
                        PublishDate = DateTime.Now.AddDays(-2),
                        WorkingHoursStart = new TimeSpan(09, 00, 00),
                        WorkingHoursEnd = new TimeSpan(16, 30, 00),
                        Job_JobBenefits = _context.Job_JobBenefits.Skip(2).Take(2).ToList(),
                    },
                    new Job
                    {
                        JobTitle = "Office Secretary",
                        Title = "Office Secretary",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        CreatedDate = DateTime.Now.AddDays(-2),
                        CreatedBy = jobOwner,
                        Description = "Usual secretarial duties",
                        HolidayEntitlement = 12,
                        HoursPerWeek = 60,
                        IsActive = true,
                        JobType = _context.JobTypes.Take(1).First(),
                        MaxSalary = 20000M,
                        MinSalary = 18000,
                        PublishDate = DateTime.Now.AddDays(1),
                        WorkingHoursStart = new TimeSpan(07, 00, 00),
                        WorkingHoursEnd = new TimeSpan(18, 30, 00),
                        Job_JobBenefits = _context.Job_JobBenefits.Skip(2).Take(2).ToList(),
                    },
                    new Job
                    {
                        JobTitle = "LGV Driver",
                        Title = "LGV Driver",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        CreatedDate = DateTime.Now.AddDays(-2),
                        CreatedBy = jobOwner,
                        Description = "Drive big trucks to places, unload the required stock, rinse and repeat",
                        HolidayEntitlement = 31,
                        HoursPerWeek = 50,
                        IsActive = true,
                        JobType = _context.JobTypes.Take(2).First(),
                        MaxSalary = 32000M,
                        MinSalary = 24000,
                        PublishDate = DateTime.Now.AddDays(-4),
                        WorkingHoursStart = new TimeSpan(05, 00, 00),
                        WorkingHoursEnd = new TimeSpan(15, 30, 00)
                    },
                    new Job
                    {
                        JobTitle = "Fudge Packer",
                        Title = "Fudge Packer",
                        ClosingDate = DateTime.Now.AddMonths(-1),
                        CreatedDate = DateTime.Now.AddDays(-7),
                        CreatedBy = jobOwner,
                        Description = "To pack as much fudge as possible",
                        HolidayEntitlement = 20,
                        HoursPerWeek = 20,
                        IsActive = true,
                        JobType = _context.JobTypes.First(),
                        MaxSalary = 22000M,
                        MinSalary = 2000,
                        PublishDate = DateTime.Now,
                        WorkingHoursStart = new TimeSpan(09, 00, 00),
                        WorkingHoursEnd = new TimeSpan(15, 30, 00)
                    },
                    new Job
                    {
                        JobTitle = "CTO",
                        Title = "CTO (Chief Technology Officer)",
                        ClosingDate = DateTime.Now.AddMonths(10),
                        CreatedDate = DateTime.Now.AddDays(-2),
                        CreatedBy = jobOwner,
                        Description = "Are you intrigued by the advances in big data? Are you excited by the opportunity to develop a company’s technological backbone from the ground up? If so, we can’t wait to hear from you. Our start-up marketing company needs a CTO (Chief Technology Officer) who can provide technical leadership and training for the rest of the company. The ideal applicant will have an organised and creative mindset that drives him or her to find innovative solutions to common problems. If you have a background in marketing and have the skills necessary to fulfil the obligations of a C-level position, we want to hear from you.",
                        HolidayEntitlement = 62,
                        HoursPerWeek = 20,
                        IsActive = true,
                        JobType = _context.JobTypes.Take(1).First(),
                        MaxSalary = 220000M,
                        MinSalary = 180000,
                        PublishDate = DateTime.Now.AddDays(-3),
                        WorkingHoursStart = new TimeSpan(09, 00, 00),
                        WorkingHoursEnd = new TimeSpan(15, 30, 00)
                    }
                );
                await _context.SaveChangesAsync();
            }
        }
    }
}