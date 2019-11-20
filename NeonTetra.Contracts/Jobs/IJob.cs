using System;

namespace NeonTetra.Contracts.Jobs
{
    public interface IJob
    {
    }

    public interface ILoggingJob : IJob
    {
        void Information(Type parentType, string message);
    }
}