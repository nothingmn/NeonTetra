using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class Server : IServer
    {
        public double Lat { get; set; } = 0;
        public double Lon { get; set; } = 0;
        public DateTime? Sunrise { get; set; }
        public DateTime? Sunrset { get; set; }
    }
}