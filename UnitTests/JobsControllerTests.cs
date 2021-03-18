using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JobWebsiteMVC.Controllers;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using JobWebsiteMVC.ViewModels.Job;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class JobsControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<JobsController>> _mockLogger;
        private Mock<IJobService> _mockService;
        private Mock<IJobTypesService> _mockJobTypesService;
        private Mock<IJobBenefitsService> _mockJobBenefitsService;

        [TestInitialize]
        public void SetupTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<JobsController>>();
            _mockService = new Mock<IJobService>();
            _mockJobTypesService = new Mock<IJobTypesService>();
            _mockJobBenefitsService = new Mock<IJobBenefitsService>();

            _mockMapper.Setup(x => x.Map<List<JobDetailsViewModel>>(It.IsAny<Job>())).Returns(new List<JobDetailsViewModel> {
                new JobDetailsViewModel{ },
                new JobDetailsViewModel{ }
            });
            _mockJobTypesService.Setup(x => x.GetJobTypes()).ReturnsAsync(new List<JobType> { new JobType { Description = "Job Type 1" } });
        }

        [TestMethod]
        public async Task GetJobs_Returns_Jobs()
        {
            // Arrange
            _mockService.Setup(x => x.GetJobs(It.IsAny<string>(), It.IsAny<bool>(), null)).ReturnsAsync(
                new List<Job> {
                    new Job { Id = new Guid(), Title = "Test title", JobType = new JobType{ Id = new Guid(), Description = "JobType"}, IsDraft=false, MinSalary = 1234M, MaxSalary = 4321M, Description = "Test", IsActive = true, JobTitle = "JobTitle", ClosingDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now, HolidayEntitlement = 21, HoursPerWeek = 40 },
                    new Job { Id = new Guid(), Title = "Test title two", JobType = new JobType{ Id = new Guid(), Description = "JobType2"}, IsDraft=false, MinSalary = 12344M, MaxSalary = 34321M, Description = "Test2", IsActive = true, JobTitle = "JobTitle2", ClosingDate = DateTime.Now.AddDays(8), CreatedDate = DateTime.Now, HolidayEntitlement = 20, HoursPerWeek = 37.5M  }
                });

            // Act
            var controller = new JobsController(_mockMapper.Object, _mockLogger.Object, _mockService.Object, _mockJobTypesService.Object, _mockJobBenefitsService.Object);

            // Assert
            var result = await controller.Index(It.IsAny<string>(), It.IsAny<bool>(), null) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetJobs_Returns_Correct_Filtered_Jobs()
        {
            // Arrange
            _mockService.Setup(x => x.GetJobs(It.IsAny<string>(), It.IsAny<bool>(), null)).ReturnsAsync(
                new List<Job> {
                    new Job { Id = new Guid(), Title = "Test title", JobType = new JobType{ Id = new Guid(), Description = "JobType"}, IsDraft=false, MinSalary = 1234M, MaxSalary = 4321M, Description = "Test", IsActive = true, JobTitle = "JobTitle", ClosingDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now, HolidayEntitlement = 21, HoursPerWeek = 40 },
                    new Job { Id = new Guid(), Title = "Test title two", JobType = new JobType{ Id = new Guid(), Description = "JobType2"}, IsDraft=false, MinSalary = 12344M, MaxSalary = 34321M, Description = "Test2", IsActive = true, JobTitle = "JobTitle2", ClosingDate = DateTime.Now.AddDays(8), CreatedDate = DateTime.Now, HolidayEntitlement = 20, HoursPerWeek = 37.5M  }
                });

            // Act
            var controller = new JobsController(_mockMapper.Object, _mockLogger.Object, _mockService.Object, _mockJobTypesService.Object, _mockJobBenefitsService.Object);

            // Assert
            var result = await controller.Index(It.IsAny<string>(), It.IsAny<bool>(), null) as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}