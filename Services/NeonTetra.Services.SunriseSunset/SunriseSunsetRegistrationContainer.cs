using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Services;

namespace NeonTetra.Services.SunriseSunset
{
    public class SunriseSunsetRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public void Register(IDIContainer container)
        {
            container.Register<ISunriseSunset, CoordinateSharpSunriseSunset>();
        }

        public Task ExecutePostRegistrationStep(IDIContainer container, CancellationToken cancellationToken = default(CancellationToken))
        {
            var config = container.Resolve<IConfiguration>();
            var server = container.Resolve<IServer>();
            server.Lat = config.GetValueOrDefault("Server:Location:Lat", 0.0);
            server.Lon = config.GetValueOrDefault("Server:Location:Lon", 0.0);

            if (server.Lat != 0 && server.Lon != 0)
            {
                var ss = container.Resolve<ISunriseSunset>();
                server.Sunrise = ss.GetSunrise(server.Lat, server.Lon);
                server.Sunrset = ss.GetSunset(server.Lat, server.Lon);
            }

            return Task.CompletedTask;
        }
    }
}