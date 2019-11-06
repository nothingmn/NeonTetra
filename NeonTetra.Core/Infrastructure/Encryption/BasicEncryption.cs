using NeonTetra.Contracts.Infrastructure.Encryption;

namespace NeonTetra.Core.Infrastructure.Encryption
{
    public class BasicEncryption : IEncryption
    {
        private readonly Decryptor _decryptor;

        private readonly Encryptor _encryptor;

        public BasicEncryption(EncryptionAlgorithm algorithm)
        {
            _encryptor = new Encryptor(algorithm);
            _decryptor = new Decryptor(algorithm);
        }

        public byte[] Encrypt(byte[] input, IEncryptionKey key = null)
        {
            if (key == null) key = Key;
            return _encryptor.Encrypt(input, key);
        }

        public byte[] Decrypt(byte[] input, IEncryptionKey key = null)
        {
            if (key == null) key = Key;
            return _decryptor.Decrypt(input, key);
        }

        public IEncryptionKey Key { get; set; }
    }
}