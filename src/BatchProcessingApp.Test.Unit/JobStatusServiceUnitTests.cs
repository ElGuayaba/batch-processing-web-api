using BatchProcessingApp.DataAccess.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BatchProcessingApp.Test.Unit
{
    public class JobStatusServiceUnitTests
    {
        public JobStatusServiceUnitTests()
        {
        }

        [Fact]
        public async Task AddStatus_Success()
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();
            var jobStatusService = new JobStatusService();

            // Act
            await jobStatusService.AddStatusAsync(dummyGuid, 10);

            // Assert
            jobStatusService.Status.Should().NotBeEmpty();
            jobStatusService.Status[dummyGuid].TotalItems.Should().Be(10);
        }

        [Fact]
        public async Task AccumulateProcessed_Success()
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();
            var jobStatusService = new JobStatusService();

            // Act
            await jobStatusService.AddStatusAsync(dummyGuid, 10);
            await jobStatusService.AccumulateProcessedAsync(dummyGuid);

            // Assert
            jobStatusService.Status.Should().NotBeEmpty();
            jobStatusService.Status[dummyGuid].ProcessedItems.Should().Be(1);
        }

        [Fact]
        public async Task AccumulateErrors_Success()
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();
            var jobStatusService = new JobStatusService();

            // Act
            await jobStatusService.AddStatusAsync(dummyGuid, 10);
            await jobStatusService.AccumulateErrorsAsync(dummyGuid);

            // Assert
            jobStatusService.Status.Should().NotBeEmpty();
            jobStatusService.Status[dummyGuid].Errors.Should().Be(1);
        }

        [Fact]
        public async Task GetStatus_Success()
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();
            var jobStatusService = new JobStatusService();

            // Act
            await jobStatusService.AddStatusAsync(dummyGuid, 10);
            var result = await jobStatusService.GetStatusAsync(dummyGuid);

            // Assert
            jobStatusService.Status.Should().NotBeEmpty();
            result.TotalItems.Should().Be(10);
            result.ProcessedItems.Should().Be(0);
            result.Errors.Should().Be(0);
        }
    }
}
