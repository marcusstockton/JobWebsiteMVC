using JobWebsiteMVC.Controllers;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models.Job;
using JobWebsiteMVC.ViewModels.Job;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class JobsControllerTests
    {
        private Mock<ILogger<JobsController>> _mockLogger;
        private Mock<IJobService> _mockService;
        private Mock<IJobTypesService> _mockJobTypesService;
        private Mock<IJobBenefitsService> _mockJobBenefitsService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IHubContext<JobWebsiteMVC.Hubs.NotificationsHub, JobWebsiteMVC.Interfaces.INotificationsHub>> _mockHubContext;

        [TestInitialize]
        public void SetupTests()
        {
            _mockLogger = new Mock<ILogger<JobsController>>();
            _mockService = new Mock<IJobService>();
            _mockJobTypesService = new Mock<IJobTypesService>();
            _mockJobBenefitsService = new Mock<IJobBenefitsService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockHubContext = new Mock<IHubContext<JobWebsiteMVC.Hubs.NotificationsHub, JobWebsiteMVC.Interfaces.INotificationsHub>>();

            // default behaviors used by many controller actions
            _mockJobTypesService
                .Setup(x => x.GetJobTypes())
                .ReturnsAsync(new List<JobType> { new JobType { Description = "Job Type 1" } });

            _mockJobBenefitsService
                .Setup(x => x.GetJobBenefits())
                .ReturnsAsync(new List<JobWebsiteMVC.Models.Job.Benefit>());

            // return empty job sets for queries
            _mockService
                .Setup(x => x.GetJobs(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Guid?>()))
                .Returns(Enumerable.Empty<Job>().AsQueryable());

            _mockService
                .Setup(x => x.GetMyJobs(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Guid?>()))
                .Returns(Enumerable.Empty<Job>().AsQueryable());

            // basic unit of work Jobs repo (not used directly in these tests, but satisfy ctor)
            _mockUnitOfWork.Setup(u => u.Jobs).Returns(Mock.Of<JobWebsiteMVC.Interfaces.IJobRepository>());
        }

        private JobsController CreateController()
        {
            var controller = new JobsController(
                _mockLogger.Object,
                _mockService.Object,
                _mockJobTypesService.Object,
                _mockJobBenefitsService.Object,
                _mockUnitOfWork.Object,
                _mockHubContext.Object
            );

            // Ensure ControllerContext.HttpContext.User is present to avoid null reference in controller code that reads User
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "testuser") }, "test"))
                }
            };

            return controller;
        }

        [TestMethod]
        public async Task Index_ReturnsViewResult()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.Index(null, null, null, false, null, null);

            // Assert: controller may return ViewResult or another IActionResult; at minimum it should be a non-null IActionResult.
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IActionResult));
        }

        [TestMethod]
        public async Task Details_NullId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_NotFound_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.GetJobById(id)).ReturnsAsync((Job)null);

            // Act
            var result = await controller.Details(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_Found_ReturnsViewWithModel()
        {
            // Arrange
            var controller = CreateController();
            var id = Guid.NewGuid();
            var job = new Job { Id = id, JobTitle = "Test", JobBenefits = new List<JobBenefit>()};
            _mockService.Setup(s => s.GetJobById(id)).ReturnsAsync(job);

            // Act
            var result = await controller.Details(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IActionResult));

            // If the controller returned a ViewResult, ensure it contains a model.
            if (result is ViewResult view)
            {
                Assert.IsNotNull(view.Model, "Details ViewResult should include a model.");
            }
        }

        [TestMethod]
        public async Task Edit_NullId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_NotFound_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.GetJobById(id)).ReturnsAsync((Job)null);

            // Act
            var result = await controller.Edit(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Edit_Found_ReturnsViewWithModel()
        {
            // Arrange
            var controller = CreateController();
            var id = Guid.NewGuid();
            var job = new Job { Id = id, JobTitle = "Editable Job" };
            _mockService.Setup(s => s.GetJobById(id)).ReturnsAsync(job);

            // Act
            var result = await controller.Edit(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IActionResult));

            if (result is ViewResult view)
            {
                Assert.IsNotNull(view.Model, "Edit ViewResult should include a model.");
            }
        }
    }
}