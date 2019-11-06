using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Logging
{
    public interface IInstrumentationRepository
    {
        Task Push(IInstrumentationEntity entity);
    }

    public interface IInstrumentationEntity
    {
        string EventType { get; set; }
        Exception Exception { get; set; }
        string RawMessage { get; set; }
        string FormattedMessage { get; set; }
        IDictionary<string, string> Properties { get; set; }
        IDictionary<string, double> Metrics { get; set; }
        DateTimeOffset Timestamp { get; set; }
    }
}