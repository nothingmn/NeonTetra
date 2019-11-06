namespace NeonTetra.Contracts.Infrastructure
{
    public interface ITcpClientFactory
    {
        ITcpClient CreateClient(IServerConnection serverConnection);
    }
}