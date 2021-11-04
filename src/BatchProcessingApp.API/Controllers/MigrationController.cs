using BatchProcessingApp.Application.Contracts;
using BatchProcessingApp.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BatchProcessingApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MigrationController : ControllerBase
    {
        private readonly ILogger<MigrationController> logger;
        private readonly IJobSchedulingService jobSchedulingService;

        public MigrationController(
            ILogger<MigrationController> logger,
            IJobSchedulingService jobSchedulingService)
        {
            this.logger = logger;
            this.jobSchedulingService = jobSchedulingService;
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleMigration(JobTypeEnum jobType, IFormFile csvFile, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid || csvFile is null)
            {
                return BadRequest();
            }

            var result = await jobSchedulingService.EnqueueJobAsync(jobType, csvFile);

            if (result is null)
            {
                return UnprocessableEntity();
            }

            return Ok(new { JobId = result });
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetMigrationStatus(string jobId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid || !Guid.TryParse(jobId, out var jobIdParsed))
            {
                return BadRequest();
            }

            var result = await jobSchedulingService.CheckJobAsync(jobIdParsed);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
