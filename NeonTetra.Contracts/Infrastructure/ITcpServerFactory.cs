using System.Threading.Tasks;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface ITcpServerFactory
    {
        Task<ITcpServer> CreateServer(IServerConnection serverConnection);
    }
}