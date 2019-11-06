namespace NeonTetra.Contracts.Infrastructure.Encryption
{
    public interface IEncryptionKeyReadProvider
    {
        IEncryptionKey GetKeyForIntent(EncryptionIntent intent);
    }

    public interface IEncryptionKeyWriteProvider
    {
        void WriteKeyForIntent(EncryptionIntent intent, IEncryptionKey key);
    }
}