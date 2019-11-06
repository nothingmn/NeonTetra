using System;
using System.Collections.Generic;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Core.Logging
{
    public class InstrumentationEntity : IInstrumentationEntity
    {
        public string EventType { get; set; } = "Event";
        public Exception Exception { get; set; }
        public string RawMessage { get; set; }
        public string FormattedMessage { get; set; }
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, double> Metrics { get; set; } = new Dictionary<string, double>();

        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    }
}