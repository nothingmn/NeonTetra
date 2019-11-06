using NeonTetra.Contracts.Infrastructure.Encryption;

namespace NeonTetra.Core.Infrastructure.Encryption
{
    public class BasicEncryptionFactory : IEncryptionFactory
    {
        private readonly IEncryptionKeyReadProvider _encryptionKeyReadProvider;

        public BasicEncryptionFactory(IEncryptionKeyReadProvider encryptionKeyReadProvider)
        {
            _encryptionKeyReadProvider = encryptionKeyReadProvider;
        }

        public IEncrypter EncrypterForIntent(EncryptionIntent intent)
        {
            var key = _encryptionKeyReadProvider.GetKeyForIntent(intent);
            var e = new BasicEncryption(key.Algorithm)
            {
                Key = key
            };
            return e;
        }

        public IDecrypter DencrypterForIntent(EncryptionIntent intent)
        {
            var key = _encryptionKeyReadProvider.GetKeyForIntent(intent);
            var e = new BasicEncryption(key.Algorithm)
            {
                Key = key
            };
            return e;
        }

        public IEncryption EncryptionForIntent(EncryptionIntent intent)
        {
            var key = _encryptionKeyReadProvider.GetKeyForIntent(intent);
            var e = new BasicEncryption(key.Algorithm)
            {
                Key = key
            };
            return e;
        }

        public IEncrypter EncrypterForAlgorithm(EncryptionAlgorithm algorithm)
        {
            return new BasicEncryption(algorithm);
        }

        public IDecrypter DencrypterForAlgorithm(EncryptionAlgorithm algorithm)
        {
            return new BasicEncryption(algorithm);
        }

        public IEncryption EncryptionForAlgorithm(EncryptionAlgorithm algorithm)
        {
            return new BasicEncryption(algorithm);
        }
    }
}