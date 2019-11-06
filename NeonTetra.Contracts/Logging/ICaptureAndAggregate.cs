using System;

namespace NeonTetra.Contracts.Logging
{
    public interface ICaptureAndAggregate
    {
        DateTimeOffset StartDateTimeOffset { get; set; }
        void Start(Func<double> callback);

        double TotalAverage();

        double Last5SecondsAverage();
    }
}