using BatchProcessingApp.Common.Models;
using System;
using System.Threading.Tasks;

namespace BatchProcessingApp.DataAccess.Contracts
{
    public interface IJobStatusService
    {
        Task AddStatusAsync(Guid jobId, int totalItems);

        Task AccumulateProcessedAsync(Guid jobId);

        Task AccumulateErrorsAsync(Guid jobId);

        Task<JobState> GetStatusAsync(Guid jobId);
    }
}
