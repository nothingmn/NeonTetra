using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class TimedBufferBatchWithErrorRetries<T> : ITimedBufferBatchWithErrorRetries<T>
    {
        private readonly ConcurrentDictionary<T, int> _failedItems = new ConcurrentDictionary<T, int>();
        private Func<T, int, Task> _failure;

        private int _totalFailedMessages;
        private int _totalHardFailedMessages;

        private int _totalSuccessfulMessages;

        public TimedBufferBatchWithErrorRetries(ITimedBufferBatch<T> input, ITimedBufferBatch<T> error)
        {
            Input = input;
            Error = error;

            Error.BatchSize = 100;
            Error.TimerDueTime = TimeSpan.FromSeconds(10);
            Error.TimerPeriod = TimeSpan.FromSeconds(10);
        }

        public ITimedBufferBatch<T> Input { get; set; }
        public ITimedBufferBatch<T> Error { get; set; }

        public int TotalHardFailedMessages => _totalHardFailedMessages;

        public int TotalSuccessfulMessages => _totalSuccessfulMessages;

        public int TotalFailedMessages => _totalFailedMessages;

        public int CurrentFailedMessages => _failedItems.Count;

        public double TotalSuccessRate =>
            TotalSuccessfulMessages / (double) (TotalFailedMessages + TotalSuccessfulMessages) * 100;

        public int MaxFailuresBeforeHardFailure { get; set; } = 5;

        public void Post(T item)
        {
            Input.Post(item);
        }

        public void NotifySuccess(T item)
        {
            Interlocked.Increment(ref _totalSuccessfulMessages);
            if (_failedItems.ContainsKey(item))
            {
                var count = 0;
                _failedItems.TryRemove(item, out count);
            }
        }

        public void NotifyError(T item)
        {
            Interlocked.Increment(ref _totalFailedMessages);
            if (_failedItems.ContainsKey(item))
                _failedItems[item]++;
            else
                _failedItems.TryAdd(item, 1);

            if (_failedItems[item] >= MaxFailuresBeforeHardFailure)
            {
                Interlocked.Increment(ref _totalHardFailedMessages);

                var count = 0;
                _failedItems.TryRemove(item, out count);
                _failure?.Invoke(item, count);
            }
            else
            {
                Error.Post(item);
            }
        }

        public void Subscribe(Func<IEnumerable<T>, Task> action, Func<T, int, Task> failure, CancellationToken token)
        {
            _failure = failure;

            //just allow this to follow through
            Input.Subscribe(action, token);

            //subscribe when this flow triggers to requeue back into flow
            Error.Subscribe(enumerable =>
            {
                var errorItems = enumerable as T[] ?? enumerable.ToArray();
                //Console.WriteLine("error flow triggered, moving data back to input, count:" + errorItems.Count());

                foreach (var e in errorItems) Input.Post(e);
                return Task.CompletedTask;
            }, token);
        }
    }
}