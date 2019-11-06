using System;

namespace NeonTetra.Contracts.Logging
{
    public interface IInlineEventTimer
    {
        string Category { get; }
        string Method { get; }

        TimeSpan Elapsed { get; }
        double MemoryConsumption { get; }

        void Reset();
    }
}