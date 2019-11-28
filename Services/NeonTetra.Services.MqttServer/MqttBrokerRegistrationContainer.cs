using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Services.MqttServer
{
    public class MqttBrokerRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public void Register(IDIContainer container)
        {
        }

        public async Task ExecutePostRegistrationStep(IDIContainer container, CancellationToken cancellationToken = default(CancellationToken))
        {
            var config = container.Resolve<IConfiguration>();
            var enabled = config.GetValueOrDefault("MQTT:Server:Enabled", true);
            if (enabled)
            {
                var port = config.GetValueOrDefault("MQTT:Server:Port", 1889);
                var broker = new MqttBroker();
                await broker.StartAsync(container.Resolve<IAccountManager>(), port);
                container.RegisterInstance(broker);
            }
        }
    }
}