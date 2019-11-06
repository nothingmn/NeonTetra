using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NeonTetra.Contracts.Logging
{
    internal class EventTimingRecorder : IDisposable
    {
        private readonly string _category;
        private readonly ILog _log;
        private readonly string _method;
        private readonly bool _nanoSecondPrecision;
        private readonly long _startTimestamp;
        private readonly long _startWorkingSet;

        public EventTimingRecorder(ILog log, string category, string method, bool nanoSecondPrecision = false)
        {
            _log = log;
            _category = category;
            _method = method;
            _nanoSecondPrecision = nanoSecondPrecision;
            _startWorkingSet = Environment.WorkingSet;
            _startTimestamp = Stopwatch.GetTimestamp();
        }

        public void Dispose()
        {
            var metrics = new Dictionary<string, double>();

            var timestamp = Stopwatch.GetTimestamp();
            var tickDelta = timestamp - _startTimestamp;
            if (_nanoSecondPrecision)
            {
                var elapsedNs = 1000000000.0 * tickDelta / Stopwatch.Frequency;
                metrics["Elapsed Nanoseconds"] = elapsedNs;
            }
            else
            {
                var elapsedMs = 1000.0 * tickDelta / Stopwatch.Frequency;
                metrics["Elapsed Milliseconds"] = elapsedMs;
            }

            var workingSet = Environment.WorkingSet;
            var memoryDelta = workingSet - _startWorkingSet;
            metrics["Working Set"] = workingSet;
            metrics["Memory Delta"] = memoryDelta;

            _log.Event("Timing", new Dictionary<string, string>
            {
                {"Category", _category},
                {"Method", _method}
            }, metrics);
        }
    }
}