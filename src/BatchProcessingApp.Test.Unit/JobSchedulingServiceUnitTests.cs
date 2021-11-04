using BatchProcessingApp.Application.Contracts;
using BatchProcessingApp.Application.Exceptions;
using BatchProcessingApp.Application.Jobs;
using BatchProcessingApp.Application.Services;
using BatchProcessingApp.Application.Wrappers;
using BatchProcessingApp.Common.Enums;
using BatchProcessingApp.Common.Models;
using BatchProcessingApp.DataAccess.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace BatchProcessingApp.Test.Unit
{
    public class JobSchedulingServiceUnitTests
    {
        private readonly Mock<IJobStrategy> jobStrategyMock;
        private readonly Mock<ILogger<JobSchedulingService>> loggerMock;
        private readonly Mock<IJobStatusService> jobStatusServiceMock;
        private readonly Mock<HangfireBackgroundJobWrapper> backgroundJobMock;

        public JobSchedulingServiceUnitTests()
        {
            jobStrategyMock = new Mock<IJobStrategy>(MockBehavior.Strict);
            loggerMock = new Mock<ILogger<JobSchedulingService>>(MockBehavior.Loose);
            jobStatusServiceMock = new Mock<IJobStatusService>(MockBehavior.Strict);
            backgroundJobMock = new Mock<HangfireBackgroundJobWrapper>(MockBehavior.Strict);
        }

        [Fact]
        public async Task EnqueueJob_Success()
        {
            // Arrange
            IJob dummyJob = new BatchJob();
            IFormFile dummyFile = new FormFile(new MemoryStream(), 0, 0, string.Empty, string.Empty);

            jobStrategyMock.Setup(x => x.GetJobAsync(It.IsAny<JobTypeEnum>()))
                .Returns(Task.FromResult(dummyJob));
            backgroundJobMock.Setup(x => x.Enqueue(It.IsAny<Expression<Func<Task>>>()))
                .Returns(string.Empty);

            var jobSchedulingService = new JobSchedulingService(
                jobStrategyMock.Object, 
                loggerMock.Object, 
                jobStatusServiceMock.Object, 
                backgroundJobMock.Object);

            // Act
            var result = await jobSchedulingService.EnqueueJobAsync(JobTypeEnum.BatchJob, dummyFile);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task EnqueueJob_ProcessingError()
        {
            // Arrange
            IJob dummyJob = new BatchJob();
            IFormFile dummyFile = new FormFile(new MemoryStream(), 0, 0, string.Empty, string.Empty);

            jobStrategyMock.Setup(x => x.GetJobAsync(It.IsAny<JobTypeEnum>()))
                .Returns(Task.FromResult(dummyJob));
            backgroundJobMock.Setup(x => x.Enqueue(It.IsAny<Expression<Func<Task>>>()))
                .Throws(new DataProcessingException(string.Empty));

            var jobSchedulingService = new JobSchedulingService(
                jobStrategyMock.Object,
                loggerMock.Object,
                jobStatusServiceMock.Object,
                backgroundJobMock.Object);

            // Act
            var result = await jobSchedulingService.EnqueueJobAsync(JobTypeEnum.BatchJob, dummyFile);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CheckJob_Success()
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();

            jobStatusServiceMock.Setup(x => x.GetStatusAsync(dummyGuid))
                .Returns(Task.FromResult(
                    new JobState
                    {
                        TotalItems = 666,
                        Errors = 42,
                        ProcessedItems = 624
                    }));

            var jobSchedulingService = new JobSchedulingService(
                jobStrategyMock.Object,
                loggerMock.Object,
                jobStatusServiceMock.Object,
                backgroundJobMock.Object);
            // Act

            var result = await jobSchedulingService.CheckJobAsync(dummyGuid);

            // Assert
            result.TotalItems.Should().Be(666);
            result.Errors.Should().Be(42);
            result.ProcessedItems.Should().Be(624);
        }

        [Fact]
        public async Task CheckJob_KeyNotFoundError()
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();

            jobStatusServiceMock.Setup(x => x.GetStatusAsync(dummyGuid))
                .Throws(new KeyNotFoundException());

            var jobSchedulingService = new JobSchedulingService(
                jobStrategyMock.Object,
                loggerMock.Object,
                jobStatusServiceMock.Object,
                backgroundJobMock.Object);
            // Act

            var result = await jobSchedulingService.CheckJobAsync(dummyGuid);

            // Assert
            result.Should().BeNull();
        }
    }
}
