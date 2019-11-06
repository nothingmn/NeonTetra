using System.Collections.Generic;
using NeonTetra.Contracts.Cache;

namespace NeonTetra.Contracts.Configuration
{
    public interface IKnownCacheConfiguration
    {
        IDictionary<KnownCaches, IKnownCacheSetup> CacheSetups { get; set; }
    }
}