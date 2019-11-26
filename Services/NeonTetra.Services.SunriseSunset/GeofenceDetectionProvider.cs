using System;
using System.Collections.Generic;
using System.Text;
using CoordinateSharp;
using NeonTetra.Contracts.Services;
using System.Linq;

namespace NeonTetra.Services.SunriseSunset
{
    public class GeofenceDetectionProvider : IGeoFenceDetection
    {
        public bool IsPointInFence(ILocation point, IGeoFence fence)
        {
            var points = (from p in fence.Fence select new GeoFence.Point(p.Lat.Point, p.Lon.Point)).ToList();
            var f = new GeoFence(points);
            var coord = new Coordinate(point.Lat.Point, point.Lon.Point);

            return f.IsPointInPolygon(coord);
        }
    }
}