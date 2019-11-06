namespace NeonTetra.Contracts.Infrastructure
{
    public enum ClientTransports
    {
        Tcp,
        Direct
    }

    public interface IServerConnection
    {
        ClientTransports Transport { get; set; }

        string Host { get; set; }

        string Name { get; }

        int Port { get; set; }
    }
}