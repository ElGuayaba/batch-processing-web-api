using BatchProcessingApp.Common.Models;
using BatchProcessingApp.Infrastructure.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BatchProcessingApp.Test.Unit
{
    public class ProcessingServiceUnitTests
    {
        [Fact]
        public async Task ProcessItemAsync_Success()
        {
            // Arrange
            var processingService = new ProcessingService();

            // Act
            var result = await processingService.ProcessItemAsync(new DataEntry());

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ProcessItemAsync_Failure()
        {
            // Arrange
            var processingService = new ProcessingService();

            // Act
            var result = await processingService.ProcessItemAsync(new DataEntry { Detail = "error" });

            // Assert
            result.Should().BeFalse();
        }
    }
}
