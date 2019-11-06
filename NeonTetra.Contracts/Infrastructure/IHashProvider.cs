namespace NeonTetra.Contracts.Infrastructure
{
    public interface IHashProvider
    {
        byte[] Hash(byte[] input);
    }
}