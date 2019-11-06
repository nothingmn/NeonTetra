namespace NeonTetra.Contracts.Infrastructure.Encryption
{
    public interface IValidateEncryptionKeys
    {
        bool Validate(IEncryptionKey key, EncryptionAlgorithm algorithm);
    }
}