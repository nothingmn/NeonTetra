using System;
using System.Linq.Expressions;
using Hangfire;
using NeonTetra.Contracts.Services;

namespace NeonTetraWebApi.Scheduling
{
    public class BasicHangfireScheduler : IScheduler
    {
        public void RecurringJobAddOrUpdate(Expression<Action> methodCall, string cronExpression, TimeZoneInfo tzInfo = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate(methodCall, cronExpression, tzInfo, queue);
        }

        public void RecurringJobRemoveIfExists(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }

        public void RecurringTrigger(string jobId)
        {
            RecurringJob.Trigger(jobId);
        }

        public string Enqueue(Expression<Action> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }

        public string Schedule(Expression<Action> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule(methodCall, delay);
        }

        public string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt)
        {
            return BackgroundJob.Schedule(methodCall, enqueueAt);
        }
    }
}