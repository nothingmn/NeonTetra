using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Core.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace NeonTetra.DI.Containers
{
    //[RegistrationStepOrder(Order = 1)]
    public class ConfigurationRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public Task ExecutePostRegistrationStep(IDIContainer container, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public void Register(IDIContainer container)
        {
            if (!container.IsRegistered<IConfiguration>())
            {
                container.Register<IConfiguration, Configuration>();
                var config = container.Resolve<IConfiguration>();
                config.Build();
                container.RegisterInstance(config, typeof(IConfiguration));
            }
        }
    }
}