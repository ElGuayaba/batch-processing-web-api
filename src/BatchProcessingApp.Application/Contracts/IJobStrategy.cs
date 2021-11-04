using BatchProcessingApp.Application.Jobs;
using BatchProcessingApp.Common.Enums;
using System.Threading.Tasks;

namespace BatchProcessingApp.Application.Contracts
{
    public interface IJobStrategy
    {
        Task<IJob> GetJobAsync(JobTypeEnum jobType);
    }
}
