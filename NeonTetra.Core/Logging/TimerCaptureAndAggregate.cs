using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Core.Logging
{
    public class TimerCaptureAndAggregate : ICaptureAndAggregate
    {
        private Func<double> _callback;
        private IDictionary<DateTimeOffset, double> history = new ConcurrentDictionary<DateTimeOffset, double>();

        private Timer rpsTimer;
        public DateTimeOffset StartDateTimeOffset { get; set; } = DateTimeOffset.UtcNow;

        public void Start(Func<double> callback)
        {
            _callback = callback;
            rpsTimer = new Timer(CaptureRPS, this, 0, (int) TimeSpan.FromSeconds(1).TotalMilliseconds);
        }

        public double Last5SecondsAverage()
        {
            var oldest = history.FirstOrDefault();
            var newest = history.LastOrDefault();
            if (oldest.Key != newest.Key)
            {
                var timeDiff = newest.Key - oldest.Key;
                var valueDiff = newest.Value - oldest.Value;
                return valueDiff / timeDiff.TotalSeconds;
            }

            return 0;
        }

        public double TotalAverage()
        {
            var newest = history.LastOrDefault();
            if (StartDateTimeOffset != newest.Key)
            {
                var timeDiff = newest.Key - StartDateTimeOffset;
                var valueDiff = newest.Value;
                return valueDiff / timeDiff.TotalSeconds;
            }

            return 0;
        }

        private void CaptureRPS(object state)
        {
            var value = _callback.Invoke();
            var d = DateTimeOffset.UtcNow;
            if (!history.ContainsKey(d)) history.Add(d, value);

            history = new ConcurrentDictionary<DateTimeOffset, double>(from h in history
                where (d - h.Key).TotalSeconds <= 5
                select h).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}