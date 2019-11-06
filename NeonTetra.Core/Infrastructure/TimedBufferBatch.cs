using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class TimedBufferBatch<T> : ITimedBufferBatch<T>
    {
        private Timer _timer;
        private BatchBlock<T> batchBlock;

        private BufferBlock<T> bufferBlock;
        public int BufferBoundedCapacity { get; set; }
        public int BufferMaxMessagesPerTask { get; set; }

        public int BatchSize { get; set; } = 10;
        public TimeSpan TimerDueTime { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan TimerPeriod { get; set; } = TimeSpan.FromSeconds(1);

        public void Post(IEnumerable<T> items)
        {
            foreach (var t in items) Post(t);
        }

        public void Post(T item)
        {
            bufferBlock.Post(item);
        }

        public void Subscribe(Func<IEnumerable<T>, Task> action, CancellationToken token)
        {
            var bufferDataflowBlockOptions = new DataflowBlockOptions {CancellationToken = token};
            if (BufferBoundedCapacity != default(int))
                bufferDataflowBlockOptions.BoundedCapacity = BufferBoundedCapacity;
            if (BufferMaxMessagesPerTask != default(int))
                bufferDataflowBlockOptions.MaxMessagesPerTask = BufferMaxMessagesPerTask;

            bufferBlock = new BufferBlock<T>(bufferDataflowBlockOptions);

            batchBlock = new BatchBlock<T>(BatchSize);

            var actionBlock = new ActionBlock<IEnumerable<T>>(action);
            batchBlock.LinkTo(actionBlock);
            bufferBlock.LinkTo(batchBlock);

            _timer = new Timer(state => { batchBlock.TriggerBatch(); }, bufferBlock, TimerDueTime, TimerPeriod);
        }
    }
}