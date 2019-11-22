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
            var rootUser = container.Resolve<IUser>();
            rootUser.Id = "root";
            rootUser.Name = "Root";
            rootUser.UserName = "root";
            var hashProvider = container.Resolve<IHashProvider>();

            rootUser.Password = System.Convert.ToBase64String(hashProvider.Hash(System.Text.Encoding.UTF8.GetBytes("root")));

            var users = new List<IUser>() { rootUser };
            var config = container.Resolve<IConfiguration>();
            var enabled = config.GetValueOrDefault("MQTT:Server:Enabled", true);
            if (enabled)
            {
                var port = config.GetValueOrDefault("MQTT:Server:Port", 1889);
                var broker = new MqttBroker();
                await broker.StartAsync(hashProvider, port, users);
                container.RegisterInstance(broker);
            }
        }
    }
}