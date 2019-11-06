using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using NeonTetra.Contracts.Cache;
using NeonTetra.Contracts.Configuration;

namespace NeonTetra.Core.Configuration
{
    public class KnownCacheConfiguration : IKnownCacheConfiguration
    {
        public IDictionary<KnownCaches, IKnownCacheSetup> CacheSetups { get; set; } =
            ImmutableDictionary.Create<KnownCaches, IKnownCacheSetup>();
    }

    [ExcludeFromCodeCoverage]
    public class KnownCacheSetup : IKnownCacheSetup
    {
        public KnownCaches CacheName { get; set; }
        public bool IsDistributed { get; set; }
        public string InjectedServiceName { get; set; }
    }
}