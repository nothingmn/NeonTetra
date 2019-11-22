using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.Services
{
    public interface ISunriseSunset
    {
        DateTime? GetSunrise(double lat, double lon, DateTime? date = null);

        DateTime? GetSunset(double lat, double lon, DateTime? date = null);
    }
}