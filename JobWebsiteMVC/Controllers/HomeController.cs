using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Models;
using JobWebsiteMVC.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobWebsiteMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var result = new Overview
            {
                ActiveJobCount = _context.Jobs.Where(c => c.IsActive && c.ClosingDate > DateTime.Now && c.PublishDate < DateTime.Now).Count(),
                UserCount = _context.Users.Count(),
                DraftJobs = _context.Jobs.Where(x => x.IsDraft).Count(),
                FutureJobs = _context.Jobs.Where(x => x.PublishDate > DateTime.Now).Count(),
                ClosedJobs = _context.Jobs.Where(x => x.ClosingDate <= DateTime.Now).Count(),
                JobSeekingUserCount = _userManager.GetUsersInRoleAsync("JobSeeker").Result.Count(),
                EmployerCount = _userManager.GetUsersInRoleAsync("JobOwner").Result.Count()
            };

            return View(result);
        }

        public IActionResult JobsCreatedInLastMonth()
        {
            var today = DateTime.Now;
            var start = today.AddMonths(-1);
            var end = today;
            var yearArray = Enumerable.Range(0, 1 + end.Subtract(start).Days)
                                        .Select(offset => start.AddDays(offset))
                                        .ToArray();

            var jobCreatedByDateList = _context.Jobs
                .GroupBy(y => y.CreatedDate.Date)
                .Select(x => new { Date = x.Key, Count = x.Count() })
                .ToList();

            var jobCreatedByDate = new List<Tuple<string, int>>();
            foreach (var day in yearArray)
            {
                foreach (var user in jobCreatedByDateList)
                {
                    if (user.Date == day.Date)
                    {
                        jobCreatedByDate.Add(new Tuple<string, int>(day.ToString("dd MMM yy"), user.Count));
                    }
                    else
                    {
                        jobCreatedByDate.Add(new Tuple<string, int>(day.ToString("dd MMM yy"), 0));
                    }
                }
            }
            return Json(jobCreatedByDate);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}