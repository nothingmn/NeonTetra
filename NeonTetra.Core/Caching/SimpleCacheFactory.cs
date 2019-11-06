using System;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Cache;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Core.Caching
{
    // Intended use:
    // This cache is not static.  It will have a lifetime which you provide to it.  For example registering it in the DI Container as Instance with a ServiceName
    // Would give you a specific cache
    public class SimpleCacheFactory : ICacheFactory
    {
        private readonly IDIContainer _container;
        private readonly IKnownCacheConfiguration _knownCacheConfiguration;
        private readonly ILog _log;

        public SimpleCacheFactory(IDIContainer container, IKnownCacheConfiguration knownCacheConfiguration,
            ILogFactory logFactory)
        {
            _container = container;
            _knownCacheConfiguration = knownCacheConfiguration;

            _log = logFactory.CreateLog(GetType());
        }

        public ICache<TKey, TValue> CreateCache<TKey, TValue>(string name)
        {
            KnownCaches cacheName;
            if (Enum.TryParse(name, true, out cacheName)) return CreateCache<TKey, TValue>(cacheName);

            var cache = GetCache<TKey, TValue>(name);
            if (cache == null)
            {
                cache = _container.Resolve<ICache<TKey, TValue>>("DEFAULT");
                _container.RegisterInstance(cache, typeof(ICache<TKey, TValue>), name);
            }

            return cache;
        }

        public object CreateCache<TInterface>(string name, string instanceName = null)
        {
            try
            {
                var cache = _container.Resolve<TInterface>(name);
                _container.RegisterInstance(cache, typeof(TInterface), instanceName ?? name);
                return cache;
            }
            catch
            {
                return null;
            }
        }

        public ICache<TKey, TValue> CreateCache<TKey, TValue>(KnownCaches knownCache)
        {
            var name = knownCache.ToString();
            var cache = GetCache<TKey, TValue>(name);
            if (cache == null)
            {
                if (_knownCacheConfiguration.CacheSetups.ContainsKey(knownCache))
                {
                    var config = _knownCacheConfiguration.CacheSetups[knownCache];

                    //setup the cache based on config values
                    //right now we have only the service name which to grab from the DI container, later we will add things like distributed caching, etc.

                    if (config != null)
                    {
                        var serviceName = "DEFAULT";
                        if (!string.IsNullOrEmpty(config.InjectedServiceName)) serviceName = config.InjectedServiceName;
                        cache = _container.Resolve<ICache<TKey, TValue>>(serviceName);
                    }

                    if (cache != null) _container.RegisterInstance(cache, typeof(ICache<TKey, TValue>), name);
                }
                else
                {
                    //no configuration options available for this cache, so we are just going to register it plainly
                    cache = _container.Resolve<ICache<TKey, TValue>>("DEFAULT");
                    _container.RegisterInstance(cache, typeof(ICache<TKey, TValue>), name);
                }
            }

            return cache;
        }

        private ICache<TKey, TValue> GetCache<TKey, TValue>(string name)
        {
            if (_container.IsRegistered<ICache<TKey, TValue>>(name))
                return _container.Resolve<ICache<TKey, TValue>>(name);
            return default(ICache<TKey, TValue>);
        }
    }
}