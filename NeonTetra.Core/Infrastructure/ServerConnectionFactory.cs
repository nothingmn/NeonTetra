using System;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class ServerConnectionFactory : IServerConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IResolve _resolver;

        public ServerConnectionFactory(IConfiguration configuration, IResolve resolver)
        {
            _configuration = configuration;
            _resolver = resolver;
        }

        public IServerConnection GetConnection(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new NullReferenceException("name");

            var keyPrefix = string.Concat("ServerConnection:", name, ":");

            return GetConnection(_configuration.GetValue<ClientTransports>(
                    string.Concat(keyPrefix, "Transport")),
                _configuration.GetValue<string>(string.Concat(keyPrefix, "Host")),
                _configuration.GetValue<int>(string.Concat(keyPrefix, "Port")));
        }

        public IServerConnection GetConnection(ClientTransports transport, string host, int port)
        {
            var connection = _resolver.Resolve<IServerConnection>();
            connection.Host = host;
            connection.Port = port;
            connection.Transport = transport;

            return connection;
        }
    }
}