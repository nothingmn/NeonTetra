using NeonTetra.Contracts.Services;

namespace NeonTetra.Core.Services
{
    public class Location : ILocation
    {
        public IGeoPoint Lat { get; set; } = new GeoPoint();
        public IGeoPoint Lon { get; set; } = new GeoPoint();
    }
}