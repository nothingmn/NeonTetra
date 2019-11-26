using NeonTetra.Contracts.Services;

namespace NeonTetra.Core.Services
{
    public class GeoPoint : IGeoPoint
    {
        public double Point { get; set; } = 0;
    }
}