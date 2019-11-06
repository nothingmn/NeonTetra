using System.Collections.Generic;

namespace NeonTetra.Contracts.Logging
{
    public interface IMetricSink
    {
        void Track(string metricName, double metricValue, IDictionary<string, string> properties = null);

        void Track(string metricName, IEnumerable<double> metricValues, IDictionary<string, string> properties = null);
    }
}