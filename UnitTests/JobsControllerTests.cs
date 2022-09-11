using AutoMapper;
using JobWebsiteMVC.Controllers;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using JobWebsiteMVC.Profiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class JobsControllerTests
    {
        private Mapper mapper;
        private Mock<ILogger<JobsController>> _mockLogger;
        private Mock<IJobService> _mockService;
        private Mock<IJobTypesService> _mockJobTypesService;
        private Mock<IJobBenefitsService> _mockJobBenefitsService;

        [TestInitialize]
        public void SetupTests()
        {
            _mockLogger = new Mock<ILogger<JobsController>>();
            _mockService = new Mock<IJobService>();
            _mockJobTypesService = new Mock<IJobTypesService>();
            _mockJobBenefitsService = new Mock<IJobBenefitsService>();

            var myProfile = new JobProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            mapper = new Mapper(configuration);

            _mockJobTypesService.Setup(x => x.GetJobTypes()).ReturnsAsync(new List<JobType> { new JobType { Description = "Job Type 1" } });
        }

        //[TestMethod]
        //public async Task GetJobs_Returns_Jobs()
        //{
        //    // Arrange
        //    _mockService.Setup(x => x.GetJobs(It.IsAny<string>(), It.IsAny<bool>(), null)).ReturnsAsync(
        //        new IQueryable<Job> {
        //            new Job { Id = new Guid(), JobTitle = "Test title", JobType = new JobType{ Id = new Guid(), Description = "JobType"}, IsDraft=false, MinSalary = 1234M, MaxSalary = 4321M, Description = "Test", IsActive = true, ClosingDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now, HolidayEntitlement = 21, HoursPerWeek = 40 },
        //            new Job { Id = new Guid(), JobTitle = "Test title two", JobType = new JobType{ Id = new Guid(), Description = "JobType2"}, IsDraft=false, MinSalary = 12344M, MaxSalary = 34321M, Description = "Test2", IsActive = true, ClosingDate = DateTime.Now.AddDays(8), CreatedDate = DateTime.Now, HolidayEntitlement = 20, HoursPerWeek = 37.5M  }
        //        });

        //    // Act
        //    var controller = new JobsController(mapper, _mockLogger.Object, _mockService.Object, _mockJobTypesService.Object, _mockJobBenefitsService.Object);

        //    // Assert
        //    var result = await controller.Index(It.IsAny<string>(), It.IsAny<string>(),It.IsAny<string>(), It.IsAny<bool>(), null) as ViewResult;

        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public async Task GetJobs_Returns_Correct_Filtered_Jobs()
        //{
        //    // Arrange
        //    var jobType1 = Guid.NewGuid();
        //    _mockService.Setup(x => x.GetJobs(It.IsAny<string>(), It.IsAny<bool>(), null)).ReturnsAsync(
        //        new List<Job> {
        //            new Job { Id = new Guid(), JobType = new JobType{ Id = jobType1, Description = "JobType"}, IsDraft=false, MinSalary = 1234M, MaxSalary = 4321M, Description = "Test", IsActive = true, JobTitle = "JobTitle", ClosingDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now.AddDays(-1), HolidayEntitlement = 21, HoursPerWeek = 40 },
        //        });
        //    _mockJobTypesService.Setup(x => x.GetJobTypes()).ReturnsAsync(new List<JobType> { new JobType { Description = "JobType", Id = jobType1, IsActive = true } });

        //    // Act
        //    var controller = new JobsController(mapper, _mockLogger.Object, _mockService.Object, _mockJobTypesService.Object, _mockJobBenefitsService.Object);

        //    // Assert
        //    var result = await controller.Index("title", false, jobType1) as ViewResult;

        //    Assert.IsNotNull(result);
        //}
    }
}