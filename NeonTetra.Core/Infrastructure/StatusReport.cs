using System;
using System.Collections.Generic;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class StatusReport : IStatusReport
    {
        public string Provider { get; set; }
        public int InstanceId { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public StatusTypes StatusType { get; set; }
        public IDictionary<string, double> Metrics { get; set; } = new Dictionary<string, double>();
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}