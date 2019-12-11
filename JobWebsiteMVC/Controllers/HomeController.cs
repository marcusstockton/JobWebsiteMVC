using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JobWebsiteMVC.Models;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.ViewModels.Home;

namespace JobWebsiteMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var result = new Overview
            {
                ActiveJobCount = _context.Jobs.Where(c=>c.IsActive).Count(),
                UserCount = _context.Users.Count(),
                DraftJobs = _context.Jobs.Where(x=>x.IsDraft).Count(),
                FutureJobs = _context.Jobs.Where(x=>x.PublishDate > DateTime.Now).Count(),
                ClosedJobs = _context.Jobs.Where(x=>x.ClosingDate <= DateTime.Now).Count(),
                JobSeekingUserCount = _context.Users.Where(x=>x.UserType.Description == "Job Seeker").Count(),
                EmployerCount = _context.Users.Where(x=>x.UserType.Description == "Employer").Count()
            };

            return View(result);
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
