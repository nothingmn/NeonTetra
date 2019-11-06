using System.Threading;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface ITcpClient
    {
        IServerConnection ServerConnection { get; set; }
        IPacketFormatter PacketFormatter { get; set; }

        Task<byte[]> SendMessage(byte[] message, CancellationToken cancellationToken = default(CancellationToken));
    }
}