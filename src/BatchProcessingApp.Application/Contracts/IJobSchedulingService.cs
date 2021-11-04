using BatchProcessingApp.Common.Enums;
using BatchProcessingApp.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BatchProcessingApp.Application.Contracts
{
    public interface IJobSchedulingService
    {
        Task<Guid?> EnqueueJobAsync(JobTypeEnum jobType, IFormFile dataFile);

        Task<JobState> CheckJobAsync(Guid jobId);
    }
}
