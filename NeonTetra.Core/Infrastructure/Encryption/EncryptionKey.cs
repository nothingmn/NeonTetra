using NeonTetra.Contracts.Infrastructure.Encryption;

namespace NeonTetra.Core.Infrastructure.Encryption
{
    public class EncryptionKey : IEncryptionKey
    {
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public EncryptionAlgorithm Algorithm { get; set; } = EncryptionAlgorithm.Rijndael;
    }
}