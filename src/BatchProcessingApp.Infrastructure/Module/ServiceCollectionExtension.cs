using BatchProcessingApp.Infrastructure.Contracts;
using BatchProcessingApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BatchProcessingApp.Infrastructure.Module
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IProcessingService, ProcessingService>();
        }
    }
}
