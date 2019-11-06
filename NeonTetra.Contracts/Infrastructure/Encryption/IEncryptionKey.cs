namespace NeonTetra.Contracts.Infrastructure.Encryption
{
    public interface IEncryptionKey
    {
        byte[] Key { get; set; }
        byte[] IV { get; set; }

        EncryptionAlgorithm Algorithm { get; set; }
    }
}