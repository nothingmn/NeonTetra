using System;
using System.Linq.Expressions;

namespace NeonTetra.Contracts.Services
{
    public interface IScheduler
    {
        string Enqueue(Expression<Action> methodCall);

        string Schedule(Expression<Action> methodCall, TimeSpan delay);

        string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt);

        void RecurringJobAddOrUpdate(Expression<Action> methodCall, string cronExpression, TimeZoneInfo tzInfo = null, string queue = "default");

        void RecurringJobRemoveIfExists(string jobId);

        void RecurringTrigger(string jobId);
    }
}