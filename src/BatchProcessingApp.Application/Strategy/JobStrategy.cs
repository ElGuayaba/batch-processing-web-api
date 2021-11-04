using BatchProcessingApp.Application.Contracts;
using BatchProcessingApp.Application.Jobs;
using BatchProcessingApp.Common.Enums;
using System;
using System.Threading.Tasks;

namespace BatchProcessingApp.Application.Models
{
    public class JobStrategy : IJobStrategy
    {
        private readonly IServiceProvider serviceProvider;
        public JobStrategy(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<IJob> GetJobAsync(JobTypeEnum jobType)
        {
            var instancedJobType = Type.GetType("BatchProcessingApp.Application.Jobs." + jobType.ToString());

            var temp = serviceProvider.GetService(instancedJobType);

            return (IJob)temp;
        }
    }
}
