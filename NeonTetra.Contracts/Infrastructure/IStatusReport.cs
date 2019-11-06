using System;
using System.Collections.Generic;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IStatusReport
    {
        string Provider { get; set; }
        int InstanceId { get; set; }
        DateTimeOffset Timestamp { get; set; }

        StatusTypes StatusType { get; set; }

        IDictionary<string, double> Metrics { get; set; }

        IDictionary<string, string> Properties { get; set; }
    }
}