using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IServer
    {
        double Lat { get; set; }
        double Lon { get; set; }
        DateTime? Sunrise { get; set; }
        DateTime? Sunrset { get; set; }
    }
}