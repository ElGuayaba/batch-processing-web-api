using BatchProcessingApp.Common.Models;
using BatchProcessingApp.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BatchProcessingApp.DataAccess.Services
{
    public class JobStatusService : IJobStatusService
    {
        public Dictionary<Guid, JobState> Status { get; set; }

        public JobStatusService()
        {
            Status = new Dictionary<Guid, JobState>();
        }

        public async Task AddStatusAsync(Guid jobId, int totalItems)
        {
            Status.Add(jobId, new JobState
            {
                TotalItems = totalItems,
                ProcessedItems = 0,
                Errors = 0
            });
        }

        public async Task AccumulateProcessedAsync(Guid jobId)
        {
            Status[jobId].ProcessedItems++;
        }

        public async Task AccumulateErrorsAsync(Guid jobId)
        {
            Status[jobId].Errors++;
        }

        public async Task<JobState> GetStatusAsync(Guid jobId)
        {
            return Status[jobId];
        }
    }
}
