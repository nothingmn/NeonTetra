namespace NeonTetra.Contracts.Infrastructure.Encryption
{
    public interface IEncryptionFactory
    {
        IEncrypter EncrypterForIntent(EncryptionIntent intent);

        IDecrypter DencrypterForIntent(EncryptionIntent intent);

        IEncryption EncryptionForIntent(EncryptionIntent intent);

        IEncrypter EncrypterForAlgorithm(EncryptionAlgorithm algorithm);

        IDecrypter DencrypterForAlgorithm(EncryptionAlgorithm algorithm);

        IEncryption EncryptionForAlgorithm(EncryptionAlgorithm algorithm);
    }

    public enum EncryptionIntent
    {
        MessageBroker,
        UnitTest
    }
}