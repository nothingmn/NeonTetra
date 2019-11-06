using System;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface ITcpServer
    {
        IServerConnection ServerConnection { get; set; }
        IPacketFormatter PacketFormatter { get; set; }
        ILog Log { get; set; }

        bool Started { get; set; }

        Task Start(CancellationToken waitToken, Func<byte[], CancellationToken, Task<byte[]>> onReceived);
    }
}