namespace NeonTetra.Contracts.Infrastructure.Encryption
{
    public interface IEncrypter
    {
        byte[] Encrypt(byte[] input, IEncryptionKey key = null);
    }
}