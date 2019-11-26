using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Services;

namespace NeonTetra.Core.Services
{
    public class GeoFence : IGeoFence
    {
        public IList<ILocation> Fence { get; set; } = new List<ILocation>();
    }
}