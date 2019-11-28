namespace NeonTetra.Contracts.Infrastructure
{
    public enum HashProviders
    {
        UserAccountSecurity,
        Default
    }

    public interface IHash
    {
        byte[] Hash(byte[] input, HashProviders provider = HashProviders.Default);
    }

    public interface IHashProvider
    {
        byte[] Hash(byte[] input);
    }
}