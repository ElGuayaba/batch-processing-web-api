using BatchProcessingApp.Common.Models;
using BatchProcessingApp.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BatchProcessingApp.Infrastructure.Services
{
    public class ProcessingService : IProcessingService
    {
        public async Task<bool> ProcessItemAsync(DataEntry item)
        {
            // This is a mock method to mimic a service that processes the data

            Random rnd = new Random();

            var processingTime = rnd.Next(50, 500);

            await Task.Delay(processingTime);

            if (item.Detail == "error")
            {
                return false;
            }

            return true;
        }
    }
}
