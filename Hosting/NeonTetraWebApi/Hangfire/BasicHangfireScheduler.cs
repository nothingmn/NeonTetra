using System;
using Hangfire;
using NeonTetra.Contracts.Services;
using System.Linq.Expressions;

namespace NeonTetraWebApi.Hangfire
{
    public class BasicHangfireScheduler : IScheduler
    {
        public string Enqueue(Expression<Action> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }
    }
}