using NeonTetra.Contracts.Cache;

namespace NeonTetra.Contracts.Configuration
{
    public interface IKnownCacheSetup
    {
        KnownCaches CacheName { get; set; }
        string InjectedServiceName { get; set; }
        bool IsDistributed { get; set; }
    }
}