using BatchProcessingApp.Application.Contracts;
using BatchProcessingApp.Application.Exceptions;
using BatchProcessingApp.Common.Models;
using BatchProcessingApp.DataAccess.Contracts;
using BatchProcessingApp.Infrastructure.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BatchProcessingApp.Application.Jobs
{
    public class BulkJob : IJob
    {
        private readonly IProcessingService processingService;

        private readonly ILogger<BulkJob> logger;

        private IJobStatusService jobStatusService;

        public BulkJob(
            IProcessingService processingService,
            ILogger<BulkJob> logger,
            IJobStatusService jobStatusService)
        {
            this.processingService = processingService;
            this.logger = logger;
            this.jobStatusService = jobStatusService;
        }

        public async Task ExecuteAsync(List<DataEntry> dataEntries, Guid jobId)
        {
            var timestamp = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Milliseconds;
            try
            {
                await jobStatusService.AddStatusAsync(jobId, dataEntries.Count);

                logger.LogInformation($"Processing start... | {jobId}");
                await ProcessData(dataEntries, jobId);

                logger.LogInformation($"Success... | {jobId}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error with job {jobId} | {jobId}");

                throw;
            }
        }

        private async Task ProcessData(List<DataEntry> dataEntries, Guid jobId)
        {
            foreach (var item in dataEntries)
            {
                logger.LogInformation($"Processing item {item.Id}  | {jobId}");
                if (await processingService.ProcessItemAsync(item))
                {
                    await jobStatusService.AccumulateProcessedAsync(jobId);
                }
                else
                {
                    await jobStatusService.AccumulateErrorsAsync(jobId);
                    logger.LogError($"Failed to process item {JsonConvert.SerializeObject(item)}  | {jobId}");
                }
            }
        }
    }
}
