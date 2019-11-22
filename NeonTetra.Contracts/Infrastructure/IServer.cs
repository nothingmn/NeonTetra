using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IServer
    {
        string Name { get; set; }
        bool Is64BitOperatingSystem { get; set; }
        OperatingSystem OSVersion { get; set; }
        int ProcessorCount { get; set; }
        bool Is64BitProcess { get; set; }
        double Lat { get; set; }
        double Lon { get; set; }
        DateTime? Sunrise { get; set; }
        DateTime? Sunrset { get; set; }
    }
}