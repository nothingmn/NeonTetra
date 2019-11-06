using System.Threading.Tasks;

namespace NeonTetra.Contracts.Services
{
    public enum KnownFeatureFlags
    {
    }

    public interface IFeatureFlagProvider
    {
        Task<bool> IsEnabled(string key);

        Task<bool> IsEnabled(KnownFeatureFlags key);
    }
}