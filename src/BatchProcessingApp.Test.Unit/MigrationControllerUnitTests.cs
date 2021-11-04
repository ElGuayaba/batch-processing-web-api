using BatchProcessingApp.Application.Contracts;
using BatchProcessingApp.Common.Enums;
using BatchProcessingApp.Common.Models;
using BatchProcessingApp.WebAPI.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BatchProcessingApp.Test.Unit
{
    public class MigrationControllerUnitTests
    {
        private readonly Mock<ILogger<MigrationController>> loggerMock;
        private readonly Mock<IJobSchedulingService> jobSchedulingServiceMock;

        public MigrationControllerUnitTests()
        {
            loggerMock = new Mock<ILogger<MigrationController>>(MockBehavior.Loose);
            jobSchedulingServiceMock = new Mock<IJobSchedulingService>(MockBehavior.Strict);
        }

        [Fact]
        public async Task ScheduleMigration_Success()
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();
            IFormFile dummyFile = new FormFile(new MemoryStream(), 0, 0, string.Empty, string.Empty);

            jobSchedulingServiceMock.Setup(x => x.EnqueueJobAsync(It.IsAny<JobTypeEnum>(), It.IsAny<IFormFile>()))
                .Returns(Task.FromResult((Guid?)dummyGuid));

            var controller = new MigrationController(loggerMock.Object, jobSchedulingServiceMock.Object);

            // Act
            var result = await controller.ScheduleMigration(JobTypeEnum.BatchJob, dummyFile, CancellationToken.None);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ScheduleMigration_UnprocessableEntity()
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();
            IFormFile dummyFile = new FormFile(new MemoryStream(), 0, 0, string.Empty, string.Empty);

            jobSchedulingServiceMock.Setup(x => x.EnqueueJobAsync(It.IsAny<JobTypeEnum>(), It.IsAny<IFormFile>()))
                .Returns(Task.FromResult((Guid?)null));

            var controller = new MigrationController(loggerMock.Object, jobSchedulingServiceMock.Object);

            // Act
            var result = await controller.ScheduleMigration(JobTypeEnum.BatchJob, dummyFile, CancellationToken.None);

            // Assert
            result.Should().BeOfType<UnprocessableEntityResult>();
        }

        [Fact]
        public async Task GetMigrationStatus_Success()
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();

            jobSchedulingServiceMock.Setup(x => x.CheckJobAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new JobState()));

            var controller = new MigrationController(loggerMock.Object, jobSchedulingServiceMock.Object);

            // Act
            var result = await controller.GetMigrationStatus(dummyGuid.ToString(), CancellationToken.None);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetMigrationStatus_BadRequest()
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();

            var controller = new MigrationController(loggerMock.Object, jobSchedulingServiceMock.Object);

            // Act
            var result = await controller.GetMigrationStatus(string.Empty, CancellationToken.None);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
