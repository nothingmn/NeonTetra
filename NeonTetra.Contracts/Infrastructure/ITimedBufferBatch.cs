using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface ITimedBufferBatch<T>
    {
        int BatchSize { get; set; }
        int BufferBoundedCapacity { get; set; }
        int BufferMaxMessagesPerTask { get; set; }
        TimeSpan TimerDueTime { get; set; }
        TimeSpan TimerPeriod { get; set; }

        void Post(T item);

        void Post(IEnumerable<T> items);

        void Subscribe(Func<IEnumerable<T>, Task> action, CancellationToken token);
    }
}