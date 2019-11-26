using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Jobs;
using NeonTetra.Contracts.Services;

namespace NeonTetra.Services.SunriseSunset
{
    public class SunriseSunsetRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public void Register(IDIContainer container)
        {
            container.Register<ISunriseSunset, CoordinateSharpSunriseSunset>();
            container.Register<IGeoFenceDetection, GeofenceDetectionProvider>();
        }

        public Task ExecutePostRegistrationStep(IDIContainer container, CancellationToken cancellationToken = default(CancellationToken))
        {
            var config = container.Resolve<IConfiguration>();
            var server = container.Resolve<IServer>();

            server.Location = container.Resolve<ILocation>();

            server.Location.Lat.Point = config.GetValueOrDefault("Server:Location:Lat", 0.0);
            server.Location.Lon.Point = config.GetValueOrDefault("Server:Location:Lon", 0.0);

            if (server.Location.Lat.Point != 0 && server.Location.Lon.Point != 0)
            {
                var ss = container.Resolve<ISunriseSunset>();
                server.Sunrise = ss.GetSunrise(server.Location.Lat.Point, server.Location.Lon.Point);
                server.Sunrset = ss.GetSunset(server.Location.Lat.Point, server.Location.Lon.Point);
            }

            if (container.IsRegistered<IScheduler>())
            {
                var scheduler = container.Resolve<IScheduler>();
                var updateJob = container.Resolve<IUpdateServerPropertiesJob>();
                var cron = container.Resolve<ICronExpressions>();

                scheduler.RecurringJobAddOrUpdate(() => updateJob.UpdateServerProperties(), cron.Daily(12), TimeZoneInfo.Local);
            }
            return Task.CompletedTask;
        }
    }
}