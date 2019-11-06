using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Cache;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Logging;
using NeonTetra.Contracts.Serialization;
using NeonTetra.Core.Infrastructure;

namespace NeonTetra.DI.Containers
{
    public class ResourceCompilerRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public Task ExecutePostRegistrationStep(IDIContainer container,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            container.Resolve<IResourceCompiler>().Compile();
            return Task.CompletedTask;
        }

        public void Register(IDIContainer container)
        {
            if (!container.IsRegistered<ISerialize>()) new CoreServicesRegistrationContainer().Register(container);
            if (!container.IsRegistered<ILogFactory>()) new LoggingRegistrationContainer().Register(container);
            if (!container.IsRegistered<ICacheFactory>()) new CacheRegistrationContainer().Register(container);

            container.Register<IResourceCompiler, ResourceCompiler>();
            container.Register<INeonTetraResources, NeonTetraResourceCache>();
            container.Register<ISpecificCultureTranslations, SpecificCultureTranslations>("DEFAULT");
        }
    }
}