using NeonTetra.Contracts;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class TcpClientFactory : ITcpClientFactory
    {
        private readonly IResolve _resolver;

        public TcpClientFactory(IResolve resolver)
        {
            _resolver = resolver;
        }

        public ITcpClient CreateClient(IServerConnection serverConnection)
        {
            var client = _resolver.Resolve<ITcpClient>();
            client.ServerConnection = serverConnection;
            client.PacketFormatter = new EndOfTransmissionPacketFormatter();
            return client;
        }
    }
}