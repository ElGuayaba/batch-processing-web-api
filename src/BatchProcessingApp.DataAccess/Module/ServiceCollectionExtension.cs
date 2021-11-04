using BatchProcessingApp.DataAccess.Contracts;
using BatchProcessingApp.DataAccess.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BatchProcessingApp.DataAccess.Module
{
    public static class ServiceCollectionExtension
    {
        public static void AddDataAccessServices(this IServiceCollection services)
        {
            services.AddSingleton<IJobStatusService, JobStatusService>();
        }
    }
}
