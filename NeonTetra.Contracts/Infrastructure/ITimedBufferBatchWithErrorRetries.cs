using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface ITimedBufferBatchWithErrorRetries<T>
    {
        int TotalHardFailedMessages { get; }
        int TotalSuccessfulMessages { get; }
        int TotalFailedMessages { get; }
        int CurrentFailedMessages { get; }
        double TotalSuccessRate { get; }
        int MaxFailuresBeforeHardFailure { get; set; }

        void Post(T item);

        void NotifySuccess(T item);

        void NotifyError(T item);

        void Subscribe(Func<IEnumerable<T>, Task> action, Func<T, int, Task> failure, CancellationToken token);
    }
}