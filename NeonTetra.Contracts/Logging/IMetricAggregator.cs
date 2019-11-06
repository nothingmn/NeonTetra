using System;

namespace NeonTetra.Contracts.Logging
{
    public interface IMetricAggregator
    {
        void Start(TimeSpan reportInterval);

        void Track(string metricName, double metricValue);
    }
}