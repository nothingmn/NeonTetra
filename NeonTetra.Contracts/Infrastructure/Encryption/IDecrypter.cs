namespace NeonTetra.Contracts.Infrastructure.Encryption
{
    public interface IDecrypter
    {
        byte[] Decrypt(byte[] input, IEncryptionKey key = null);
    }
}