using System.Threading;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IStatusReportSink
    {
        Task SinkReport(IStatusReport report, CancellationToken cancellationToken);
    }
}