using System;
using CoordinateSharp;
using NeonTetra.Contracts.Services;

namespace NeonTetra.Services.SunriseSunset

{
    public class CoordinateSharpSunriseSunset : ISunriseSunset
    {
        public DateTime? GetSunrise(double lat, double lon, DateTime? date = null)
        {
            if (!date.HasValue) date = DateTime.Now;
            var c = new Coordinate(lat, lon, date.Value);
            return c.CelestialInfo.SunRise + DateTimeOffset.Now.Offset;
        }

        public DateTime? GetSunset(double lat, double lon, DateTime? date = null)
        {
            if (!date.HasValue) date = DateTime.Now;
            var c = new Coordinate(lat, lon, date.Value);
            return c.CelestialInfo.SunSet + DateTimeOffset.Now.Offset;
        }
    }
}