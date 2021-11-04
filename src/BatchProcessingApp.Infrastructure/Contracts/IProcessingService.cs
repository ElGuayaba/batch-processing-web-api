using BatchProcessingApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BatchProcessingApp.Infrastructure.Contracts
{
    public interface IProcessingService
    {
        Task<bool> ProcessItemAsync(DataEntry item);
    }
}
