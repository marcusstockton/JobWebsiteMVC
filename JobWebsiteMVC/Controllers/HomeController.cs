using JobWebsiteMVC.Data;
using JobWebsiteMVC.Models;
using JobWebsiteMVC.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            var now = DateTimeOffset.Now;

            var result = new Overview
            {
                ActiveJobCount = _context.Jobs.Where(c => c.IsActive && DateTimeOffset.Compare(c.ClosingDate, now) > 0 && DateTimeOffset.Compare(c.PublishDate, now) < 0).Count(),
                UserCount = _context.Users.Count(),
                DraftJobs = _context.Jobs.Where(x => x.IsDraft).Count(),
                FutureJobs = _context.Jobs.Where(x => DateTimeOffset.Compare(x.PublishDate, now) > 0).Count(),
                ClosedJobs = _context.Jobs.Where(x => DateTimeOffset.Compare(x.ClosingDate, now) < 0).Count(),
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
                .GroupBy(y => y.CreatedDate)
                .Select(x => new { Date = x.Key, Count = x.Count() })
                .ToList();

            var jobCreatedByDate = new List<Tuple<string, int>>();
            foreach (var day in yearArray)
            {
                foreach (var user in jobCreatedByDateList)
                {
                    if (user.Date.Date == day.Date.Date)
                    {
                        jobCreatedByDate.Add(new Tuple<string, int>(day.ToString("dd MMM yy"), user.Count));
                    }
                }
                jobCreatedByDate.Add(new Tuple<string, int>(day.ToString("dd MMM yy"), 0));
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