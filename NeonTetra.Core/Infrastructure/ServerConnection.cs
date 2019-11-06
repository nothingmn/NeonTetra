using System;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class ServerConnection : IServerConnection
    {
        public ClientTransports Transport { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public string Name => string.Concat(Transport, "://", string.Concat(Host, ":", Port));

        public static IServerConnection Parse(string name)
        {
            var serverConnection = new ServerConnection();

            var t = name.Substring(0, name.IndexOf(":"));
            serverConnection.Transport = (ClientTransports) Enum.Parse(typeof(ClientTransports), t);
            serverConnection.Host = name.Substring(name.IndexOf("://", StringComparison.InvariantCultureIgnoreCase) + 3,
                name.LastIndexOf(":", StringComparison.InvariantCultureIgnoreCase));
            var port = name.Substring(name.LastIndexOf(":"));
            serverConnection.Port = int.Parse(port);
            return serverConnection;
        }
    }
}