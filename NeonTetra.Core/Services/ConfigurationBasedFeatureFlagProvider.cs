using System.Threading.Tasks;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Services;

namespace NeonTetra.Core.Services
{
    public class ConfigurationBasedFeatureFlagProvider : IFeatureFlagProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigurationBasedFeatureFlagProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> IsEnabled(string key)
        {
            var featureKey = $"FeatureFlags:{key}";
            var value = _configuration.GetValueOrDefault<bool?>(featureKey, true);
            return Task.FromResult(value ?? true);
        }

        public Task<bool> IsEnabled(KnownFeatureFlags key)
        {
            return IsEnabled(key.ToString());
        }
    }
}