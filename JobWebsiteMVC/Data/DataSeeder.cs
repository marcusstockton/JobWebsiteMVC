using Bogus;
using JobWebsiteMVC.Models;
using JobWebsiteMVC.Models.Job;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

                ApplicationUser jobOwner2 = new ApplicationUser()
                {
                    Email = "jobOwner@test.com",
                    UserName = "JobOwner",
                    DateOfBirth = new DateTime(1992, 4, 12),
                    FirstName = "JobOwner",
                    LastName = "TestUser",
                };
                await _userManager.CreateAsync(jobOwner2, "P@55w0rd1");
                await _userManager.AddToRoleAsync(jobOwner2, "JobOwner");

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


                await _context.SaveChangesAsync();
            }

            var jobOwner = await _userManager.FindByNameAsync("TestUser");
            //var jobOwner2 = await _context.Users.FindAsync("JobOwner");
            var adminUser = await _context.Users.SingleAsync(x=>x.UserName == "AdminTestUser2");
            var jobSeeker = await _context.Users.SingleAsync(x=>x.UserName == "TestUser2");

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
                    UserId = jobSeeker.Id,
                });
                await _context.SaveChangesAsync();
                //var attachment = _context.Attachments.FirstOrDefault();
                //jobOwner.Attachments.Add(attachment);
            }
            

            if (!_context.JobTypes.Any())
            {
                // Insert some job types:
                await _context.JobTypes.AddRangeAsync(
                    new JobType { Description = "Full Time", IsActive = true, CreatedBy = jobOwner },
                    new JobType { Description = "Part-Time", IsActive = true, CreatedBy = jobOwner },
                    new JobType { Description = "Contract", IsActive = true, CreatedBy = jobOwner },
                    new JobType { Description = "Permanent", IsActive = true, CreatedBy = adminUser }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.JobBenefits.Any())
            {
                // Insert some job benefits:
                await _context.AddRangeAsync(
                    new Benefit { Description = "Cycle To Work Scheme", CreatedBy = adminUser, IsActive = true },
                    new Benefit { Description = "Pension", CreatedBy = adminUser, IsActive = true },
                    new Benefit { Description = "Work From Home", CreatedBy = adminUser, IsActive = true },
                    new Benefit { Description = "Flexi-Time", CreatedBy = adminUser, IsActive = true },
                    new Benefit { Description = "Health insurance", CreatedBy = adminUser, IsActive = true },
                    new Benefit { Description = "Childcare benefits", CreatedBy = adminUser, IsActive = true },
                    new Benefit { Description = "Relocation assistance", CreatedBy = adminUser, IsActive = true },
                    new Benefit { Description = "Gym Membership", CreatedBy = adminUser, IsActive = true },
                    new Benefit { Description = "Critical Illness Cover", CreatedBy = adminUser, IsActive = true },
                    new Benefit { Description = "Death in Service Cover", CreatedBy = adminUser, IsActive = true },
                    new Benefit { Description = "Private Medical Insurance", CreatedBy = adminUser, IsActive = true }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.JobCategories.Any())
            {
                // Insert some job benefits:
                await _context.JobCategories.AddRangeAsync(
                    new JobCategory { Description = "Administrator", CreatedBy = adminUser, IsActive = true },
                    new JobCategory { Description = "Compliance officer", CreatedBy = adminUser, IsActive = true },
                    new JobCategory { Description = "Operational researcher", CreatedBy = adminUser, IsActive = true },
                    new JobCategory { Description = "Acupuncturist", CreatedBy = adminUser, IsActive = true },
                    new JobCategory { Description = "Agricultural consultant", CreatedBy = adminUser, IsActive = true },
                    new JobCategory { Description = "Oceanographer", CreatedBy = adminUser, IsActive = true },
                    new JobCategory { Description = "App developer", CreatedBy = adminUser, IsActive = true },
                    new JobCategory { Description = "Web developer", CreatedBy = adminUser, IsActive = true },
                    new JobCategory { Description = "Architect", CreatedBy = adminUser, IsActive = true },
                    new JobCategory { Description = "Land surveyor", CreatedBy = adminUser, IsActive = true },
                    new JobCategory { Description = "Cabinet maker", CreatedBy = adminUser, IsActive = true }
                );

                var bogusJobCats = new Faker<JobCategory>("en_GB")
                    .RuleFor(x => x.Description, d => d.Name.JobArea())
                    .RuleFor(x => x.CreatedBy, adminUser)
                    .RuleFor(x => x.IsActive, d => d.Random.Bool(0.9f))
                    .RuleFor(x => x.CreatedDate, d => d.Date.PastOffset());

                var jobCats = bogusJobCats.Generate(12);
                await _context.JobCategories.AddRangeAsync(jobCats);

                await _context.SaveChangesAsync();
            }

            if (!_context.Jobs.Any())
            {
                // Insert some jobs:
                await _context.AddRangeAsync(
                    new Job
                    {
                        JobTitle = "Demolition Worker",
                        ClosingDate = DateTime.Now.AddMonths(1),
                        CreatedDate = DateTime.Now.AddDays(-1),
                        PublishDate = DateTime.Now,
                        Description = "Demolition workers perform varied daily job duties depending on the demolition work that needs to be done, the size of the construction crew they work with, and the machinery and tools available to them. These core job tasks, however, are essentially the same everywhere.",
                        IsActive = true,
                        CreatedBy = jobOwner,
                        MaxSalary = 12000M,
                        MinSalary = 10000M,
                        HolidayEntitlement = 21,
                        HoursPerWeek = 40,
                        JobType = _context.JobTypes.First(),
                        JobBenefits = _context.JobBenefits.Take(2).ToList(),
                    },
                    new Job
                    {
                        JobTitle = "C# Software Devleloper",
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
                        JobBenefits = new List<JobBenefit> { new JobBenefit { JobBenefitId = _context.Benefits.Skip(1).Take(2).First().Id } }
                    },
                    new Job
                    {
                        JobTitle = "Journal Manager",
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
                        JobBenefits = new List<JobBenefit> { new JobBenefit { JobBenefitId = _context.Benefits.Skip(2).Take(1).First().Id } }
                    },
                    new Job
                    {
                        JobTitle = "Office Secretary",
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
                        JobBenefits = new List<JobBenefit> { new JobBenefit { JobBenefitId = _context.Benefits.Skip(3).Take(1).First().Id } }
                    },
                    new Job
                    {
                        JobTitle = "LGV Driver",
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
                    },
                    new Job
                    {
                        JobTitle = "Fudge Packer",
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
                        PublishDate = DateTime.Now
                    },
                    new Job
                    {
                        JobTitle = "CTO",
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
                        PublishDate = DateTime.Now.AddDays(-3)
                    }
                );
                var users = await _context.Users.ToListAsync();
                var jobTypes = await _context.JobTypes.ToListAsync();
                var jobCats = await _context.JobCategories.ToListAsync();
                var benefits = await _context.Benefits.ToListAsync();

                // Insert some bogus jobs:
                var bogusJobs = new Faker<Job>("en_GB")
                    .RuleFor(x => x.JobTitle, d => d.Name.JobTitle())
                    .RuleFor(x => x.Description, d => d.Lorem.Paragraphs(d.Random.Number(3, 9)))
                    .RuleFor(x => x.PublishDate, (d, u) => d.Date.BetweenOffset(u.CreatedDate, DateTimeOffset.Now))
                    .RuleFor(x => x.CreatedBy, d => d.PickRandom(users))
                    .RuleFor(x => x.CreatedDate, d => d.Date.PastOffset())
                    .RuleFor(x => x.MinSalary, d => d.Random.Decimal(12000, 50000))
                    .RuleFor(x => x.MaxSalary, (d, u) => d.Random.Decimal(u.MinSalary.Value, 100000))
                    .RuleFor(x => x.ClosingDate, d => d.Date.FutureOffset())
                    .RuleFor(x => x.HoursPerWeek, d => d.Random.Number(24, 40))
                    .RuleFor(x => x.IsActive, true)
                    .RuleFor(x => x.JobBenefits, d => new List<JobBenefit> { new JobBenefit { Benefit = benefits.Skip(d.Random.Number(1, 10)).First() } })
                    .RuleFor(x => x.HolidayEntitlement, d => d.Random.Decimal(12, 46))
                    .RuleFor(x => x.JobType, d => d.PickRandom(jobTypes));

                var bogusJobList = bogusJobs.Generate(200);

                await _context.Jobs.AddRangeAsync(bogusJobList);
                await _context.SaveChangesAsync();
            }
        }
    }
}