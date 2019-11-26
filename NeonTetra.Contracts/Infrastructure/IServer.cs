using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Services;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IServer
    {
        string Name { get; set; }
        bool Is64BitOperatingSystem { get; set; }
        OperatingSystem OSVersion { get; set; }
        int ProcessorCount { get; set; }
        bool Is64BitProcess { get; set; }

        ILocation Location { get; set; }
        DateTime? Sunrise { get; set; }
        DateTime? Sunrset { get; set; }
    }
}