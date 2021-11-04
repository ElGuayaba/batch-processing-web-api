using BatchProcessingApp.Application.Contracts;
using BatchProcessingApp.Application.Jobs;
using BatchProcessingApp.Application.Models;
using BatchProcessingApp.Application.Services;
using BatchProcessingApp.Application.Wrappers;
using BatchProcessingApp.DataAccess.Module;
using BatchProcessingApp.Infrastructure.Module;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BatchProcessingApp.Application.Module
{
    public static class ServicesCollectionExtension
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IJobSchedulingService, JobSchedulingService>();
            services.AddHangfire(x => x.UseSqlServerStorage(config["JobDatabase:ConnectionString"]));
            services.AddHangfireServer();
            services.AddSingleton<IJobStrategy, JobStrategy>();
            services.AddTransient<BulkJob>();
            services.AddTransient<BatchJob>();
            services.AddDataAccessServices();
            services.AddInfrastructureServices();
            services.AddSingleton<HangfireBackgroundJobWrapper>();
        }
    }
}
