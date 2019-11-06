namespace NeonTetra.Contracts.Infrastructure
{
    public interface IServerConnectionFactory
    {
        IServerConnection GetConnection(string name);

        IServerConnection GetConnection(ClientTransports transport, string host, int port);
    }
}