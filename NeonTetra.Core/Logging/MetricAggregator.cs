using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Core.Logging
{
    public class MetricAggregator : IMetricAggregator, IDisposable
    {
        private const int MaxBufferLength = 100000;

        private readonly IMetricSink _metricSink;
        private readonly ConcurrentDictionary<string, ConcurrentQueue<double>> _metricValuesByName;
        private readonly Timer _timer;

        public MetricAggregator(IMetricSink metricSink)
        {
            _metricSink = metricSink;
            _metricValuesByName =
                new ConcurrentDictionary<string, ConcurrentQueue<double>>(StringComparer.InvariantCulture);
            _timer = new Timer(FlushMetrics);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public void Start(TimeSpan reportInterval)
        {
            _timer.Change(reportInterval, reportInterval);
        }

        public void Track(string metricName, double metricValue)
        {
            var metricValues = _metricValuesByName.GetOrAdd(metricName, _ => new ConcurrentQueue<double>());
            metricValues.Enqueue(metricValue);

            // Overflow protection.
            while (metricValues.Count > MaxBufferLength) metricValues.TryDequeue(out _);
        }

        private void FlushMetrics(object _)
        {
            foreach (var kvp in _metricValuesByName)
            {
                var metricName = kvp.Key;
                var metricValueBuffer = kvp.Value;

                _metricSink.Track(metricName, GetMetricValues(metricValueBuffer));
            }
        }

        private static IEnumerable<double> GetMetricValues(ConcurrentQueue<double> metricValueBuffer)
        {
            var startingBufferSize = metricValueBuffer.Count;
            var dequeuedValueCount = 0;

            while (dequeuedValueCount <= startingBufferSize && metricValueBuffer.TryDequeue(out var metricValue))
            {
                yield return metricValue;
                dequeuedValueCount++;
            }
        }
    }
}