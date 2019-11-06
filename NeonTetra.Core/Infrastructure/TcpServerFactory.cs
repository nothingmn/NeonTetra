using System.Collections.Generic;
using System.Threading.Tasks;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Cache;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Core.Infrastructure
{
    public class TcpServerFactory : ITcpServerFactory
    {
        private readonly IResolve _resolver;
        private readonly ICache<string, ITcpServer> _serverCache;

        public TcpServerFactory(ICacheFactory cacheFactory, IResolve resolver)
        {
            _resolver = resolver;
            _serverCache = cacheFactory.CreateCache<string, ITcpServer>(KnownCaches.TcpServerInstances);
        }

        public async Task<ITcpServer> CreateServer(IServerConnection serverConnection)
        {
            if (await _serverCache.ContainsKeyAsync(serverConnection.Name))
                return await _serverCache.GetItemAsync(serverConnection.Name);

            var server = _resolver.Resolve<ITcpServer>();

            server.ServerConnection = serverConnection;
            server.PacketFormatter = new EndOfTransmissionPacketFormatter();

            server.Log = _resolver.Resolve<ILogFactory>()
                .CreateLog(new Dictionary<string, string>
                {
                    {"ApplicationName", GetType().FullName},
                    {"Category", "TcpServerFactory"},
                    {"TcpServerName", server.ServerConnection.Name},
                    {"TcpServerInstance", server.GetHashCode().ToString()}
                });

            await _serverCache.SetItemAsync(serverConnection.Name, server);

            return server;
        }
    }
}