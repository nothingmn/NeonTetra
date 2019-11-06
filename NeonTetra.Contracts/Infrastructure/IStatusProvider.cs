using System.Collections.Generic;
using System.Threading;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IStatusProvider
    {
        IList<IStatusReport> GetStatusReports();

        void PeriodicallyReport(CancellationToken token = default(CancellationToken));

        IStatusReport AssembleReport(string provider, int instanceId, StatusTypes statusType,
            IDictionary<string, double> status, IDictionary<string, string> properties = null);
    }
}