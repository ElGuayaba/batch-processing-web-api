using Hangfire;
using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BatchProcessingApp.Application.Wrappers
{
    public class HangfireBackgroundJobWrapper
    {

        public virtual string Enqueue([InstantHandle][NotNull] Expression<Func<Task>> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }
    }
}
