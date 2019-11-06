namespace NeonTetra.Contracts.Infrastructure.Encryption
{
    public interface IEncryption : IEncrypter, IDecrypter
    {
        IEncryptionKey Key { get; set; }
    }
}