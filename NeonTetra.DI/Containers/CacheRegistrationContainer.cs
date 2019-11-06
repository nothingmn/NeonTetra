using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Cache;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Core.Caching;
using NeonTetra.Core.Configuration;

namespace NeonTetra.DI.Containers
{
    public class CacheRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public Task ExecutePostRegistrationStep(IDIContainer container,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var configuration = container.Resolve<IConfiguration>();

            var knownCaches = configuration.GetValues<IKnownCacheSetup>("Cache:KnownCaches");
            if (knownCaches != null)
            {
                IDictionary<KnownCaches, IKnownCacheSetup> configValues =
                    new Dictionary<KnownCaches, IKnownCacheSetup>();

                foreach (var k in knownCaches) configValues.Add(k.CacheName, k);
                var knownConfig = container.Resolve<IKnownCacheConfiguration>();
                knownConfig.CacheSetups = configValues.ToImmutableDictionary();
                container.RegisterInstance(knownConfig, typeof(IKnownCacheConfiguration));
            }

            return Task.CompletedTask;
        }

        public void Register(IDIContainer container)
        {
            container.Register(typeof(ICache<,>), typeof(NonStaticImmutableInMemoryCache<,>));
            container.Register(typeof(ICache<,>), typeof(NonStaticImmutableInMemoryCache<,>), "DEFAULT");
            container.Register(typeof(ICache<,>), typeof(NonStaticImmutableInMemoryCache<,>), "INMEMORY_REQUESTS");
            container.Register<ICacheFactory, SimpleCacheFactory>();

            container.Register<IKnownCacheSetup, KnownCacheSetup>();
            container.Register<IKnownCacheConfiguration, KnownCacheConfiguration>();
        }
    }
}