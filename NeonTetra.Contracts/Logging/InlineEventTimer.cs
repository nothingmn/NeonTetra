using System;
using System.Diagnostics;

namespace NeonTetra.Contracts.Logging
{
    public class InlineEventTimer : IInlineEventTimer
    {
        private long _memoryUse;

        private long _startTime;

        public InlineEventTimer(string category, string method)
        {
            Category = category;
            Method = method;
            _startTime = Stopwatch.GetTimestamp();
            _memoryUse = Environment.WorkingSet;
        }

        public string Category { get; }
        public string Method { get; }

        public TimeSpan Elapsed
        {
            get
            {
                var endTimeTicks = Stopwatch.GetTimestamp();
                var deltaTicks = endTimeTicks - _startTime;
                return TimeSpan.FromMilliseconds(1000.0 * deltaTicks / Stopwatch.Frequency);
            }
        }

        public double MemoryConsumption => Environment.WorkingSet - _memoryUse;

        public void Reset()
        {
            _memoryUse = Environment.WorkingSet;
            _startTime = Stopwatch.GetTimestamp();
        }
    }
}