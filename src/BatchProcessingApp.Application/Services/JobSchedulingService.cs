using BatchProcessingApp.Application.Contracts;
using BatchProcessingApp.Application.Exceptions;
using BatchProcessingApp.Application.Wrappers;
using BatchProcessingApp.Common.Enums;
using BatchProcessingApp.Common.Models;
using BatchProcessingApp.DataAccess.Contracts;
using CsvHelper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BatchProcessingApp.Application.Services
{
    public class JobSchedulingService : IJobSchedulingService
    {
        private readonly IJobStrategy jobStrategy;

        private readonly ILogger<JobSchedulingService> logger;

        private readonly IJobStatusService jobStatusService;

        private readonly HangfireBackgroundJobWrapper backgroundJobWrapper;

        public JobSchedulingService(
            IJobStrategy jobStrategy,
            ILogger<JobSchedulingService> logger,
            IJobStatusService jobStatusService,
            HangfireBackgroundJobWrapper backgroundJobWrapper)
        {
            this.jobStrategy = jobStrategy;
            this.logger = logger;
            this.jobStatusService = jobStatusService;
            this.backgroundJobWrapper = backgroundJobWrapper;
        }

        public async Task<Guid?> EnqueueJobAsync(JobTypeEnum jobType, IFormFile dataFile)
        {
            try
            {
                var jobToTrigger = await jobStrategy.GetJobAsync(jobType);

                var dataEntries = await DeserializeCsvAsync(dataFile);

                var jobId = Guid.NewGuid();

                logger.LogDebug($"Enqueueing {jobType}...", jobId);
                backgroundJobWrapper.Enqueue(() => jobToTrigger.ExecuteAsync(dataEntries, jobId));

                return jobId;
            }
            catch (CsvDeserializeException)
            {
                return null;
            }
            catch (DataProcessingException ex)
            {
                logger.LogError(ex.Message, ex);
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError("Unknown error", ex);
                throw;
            }
        }

        public async Task<JobState> CheckJobAsync(Guid jobId)
        {
            try
            {
                logger.LogDebug("Fetching job status...");
                return await jobStatusService.GetStatusAsync(jobId);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError("Job does not exist", ex);
                return null;
            }
        }

        private async Task<List<DataEntry>> DeserializeCsvAsync(IFormFile dataFile)
        {
            try
            {
                using var reader = new StreamReader(dataFile.OpenReadStream());

                using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

                logger.LogDebug("Deserializing data...");
                return csvReader.GetRecords<DataEntry>().ToList();
            }
            catch (Exception ex)
            {
                var message = "Error deserializing file";
                logger.LogError(message, ex);
                throw new CsvDeserializeException(message, ex);
            }
        }
    }
}
