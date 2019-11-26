using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Services;

namespace NeonTetra.Core.Infrastructure
{
    public class Server : IServer
    {
        public string Name { get; set; } = System.Environment.MachineName;
        public bool Is64BitOperatingSystem { get; set; } = System.Environment.Is64BitOperatingSystem;
        public OperatingSystem OSVersion { get; set; } = System.Environment.OSVersion;
        public int ProcessorCount { get; set; } = System.Environment.ProcessorCount;
        public bool Is64BitProcess { get; set; } = System.Environment.Is64BitProcess;
        public ILocation Location { get; set; }
        public DateTime? Sunrise { get; set; }
        public DateTime? Sunrset { get; set; }
    }
}