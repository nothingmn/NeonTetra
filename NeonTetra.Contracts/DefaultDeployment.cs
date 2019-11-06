using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Contracts
{
    public class DefaultDeployment : IDeployment
    {
        public async Task Start(IDIContainer container, IDictionary<string, object> environment = null)
        {
            Container = container;
            container.InjectRegistrationModule(
                "NeonTetra.DI.Containers.DefaultDeploymentRegistrationContainer,NeonTetra.DI");
            await ExecutePostRegistrationStep(container);
        }

        public Task Stop(bool throwOnFirstException = false)
        {
            return Task.CompletedTask;
        }

        public IDIContainer Container { get; private set; }

        private async Task ExecutePostRegistrationStep(IDIContainer container)
        {
            var defaultInstance = container.Resolve<IPostRegistrationStep>("DefaultDeploymentRegistrationContainer");
            if (defaultInstance != null)
                await defaultInstance.ExecutePostRegistrationStep(container, CancellationToken.None);
        }
    }
}