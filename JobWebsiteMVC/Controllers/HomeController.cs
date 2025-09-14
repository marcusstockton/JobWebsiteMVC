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
            var today = DateTime.Today;
            var start = today.AddMonths(-1);

            // Group jobs by date (ignoring time)
            var jobCounts = _context.Jobs
                .Where(j => j.CreatedDate.Date >= start && j.CreatedDate.Date <= today)
                .GroupBy(j => j.CreatedDate.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            var result = new List<Tuple<string, int>>();
            for (var date = start; date <= today; date = date.AddDays(1))
            {
                var count = jobCounts.TryGetValue(date, out var c) ? c : 0;
                result.Add(new Tuple<string, int>(date.ToString("dd MMM yy"), count));
            }

            return Json(result);
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