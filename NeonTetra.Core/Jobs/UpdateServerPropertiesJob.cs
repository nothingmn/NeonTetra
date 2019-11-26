using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Jobs;
using NeonTetra.Contracts.Services;

namespace NeonTetra.Core.Jobs
{
    public class UpdateServerPropertiesJob : IUpdateServerPropertiesJob
    {
        private readonly IServer _server;
        private readonly ISunriseSunset _sunriseSunSet;
        private readonly IScheduler _scheduler;

        public UpdateServerPropertiesJob(IServer server, ISunriseSunset sunriseSunSet, IScheduler scheduler)
        {
            _server = server;
            _sunriseSunSet = sunriseSunSet;
            _scheduler = scheduler;
        }

        public void UpdateServerProperties()
        {
            if (_server.Location.Lat.Point != 0 && _server.Location.Lon.Point != 0)
            {
                _server.Sunrise = _sunriseSunSet.GetSunrise(_server.Location.Lat.Point, _server.Location.Lon.Point);
                _server.Sunrset = _sunriseSunSet.GetSunset(_server.Location.Lat.Point, _server.Location.Lon.Point);
            }
        }
    }
}