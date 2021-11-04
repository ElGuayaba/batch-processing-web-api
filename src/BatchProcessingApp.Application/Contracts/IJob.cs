using BatchProcessingApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BatchProcessingApp.Application.Contracts
{
    public interface IJob
    {
        Task ExecuteAsync(List<DataEntry> dataEntries, Guid jobId);
    }
}
