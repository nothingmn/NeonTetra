using NeonTetra.Contracts;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Hash
{
    public class WellKnownHashProvider : IHash
    {
        private readonly IResolve _resolver;

        public WellKnownHashProvider(IResolve resolver)
        {
            _resolver = resolver;
        }

        public byte[] Hash(byte[] input, HashProviders provider = HashProviders.Default)
        {
            IHashProvider p = null;
            if (provider == HashProviders.UserAccountSecurity)
            {
                p = _resolver.Resolve<IHashProvider>("MURMUR3");
            }
            else
            {
                p = _resolver.Resolve<IHashProvider>();
            }

            return p.Hash(input);
        }
    }
}