using System;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure.Encryption;

namespace NeonTetra.Core.Infrastructure.Encryption
{
    public class ConfigurationBasedEncryptionKeyProvider : IEncryptionKeyReadProvider, IEncryptionKeyWriteProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigurationBasedEncryptionKeyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEncryptionKey GetKeyForIntent(EncryptionIntent intent)
        {
            var key = new EncryptionKey
            {
                Key = Convert.FromBase64String(_configuration.GetValue<string>("Encryption:" + intent + ":Key")),
                IV = Convert.FromBase64String(_configuration.GetValue<string>("Encryption:" + intent + ":IV")),
                Algorithm = _configuration.GetValueOrDefault("Encryption:" + intent + ":Algorithm",
                    EncryptionAlgorithm.Rijndael)
            };
            return key;
        }

        public void WriteKeyForIntent(EncryptionIntent intent, IEncryptionKey key)
        {
            throw new NotImplementedException();
        }
    }
}