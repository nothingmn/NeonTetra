using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class StatusProvider : IStatusProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IResolve _resolver;

        public StatusProvider(IResolve resolver, IConfiguration configuration)
        {
            _resolver = resolver;
            _configuration = configuration;
        }

        public IList<IStatusReport> GetStatusReports()
        {
            var reports = new List<IStatusReport>();
            var instances = _resolver.ResolveAllInstances<IStatus>();
            if (instances == null) return reports;

            foreach (var i in from x in instances where x != null select x)
            {
                var status = i?.GetStatus();
                if (status != null) reports.Add(status);
            }

            return reports;
        }

        public void PeriodicallyReport(CancellationToken token = default(CancellationToken))
        {
            Task.Factory.StartNew(async () =>
            {
                var delay = _configuration.GetValueOrDefault("Status:Polling:Interval",
                    TimeSpan.FromSeconds(5).TotalMilliseconds);
                while (true)
                {
                    await Task.Delay((int) delay);
                    var status = GetStatusReports();

                    var sinks = _resolver.ResolveAllInstances<IStatusReportSink>();
                    foreach (var s in sinks)
                    foreach (var stat in status)
                        await s.SinkReport(stat, token);
                }
            }, token);
        }

        public IStatusReport AssembleReport(string provider, int instanceId, StatusTypes statusType,
            IDictionary<string, double> status, IDictionary<string, string> properties = null)
        {
            var report = _resolver.Resolve<IStatusReport>();

            report.Provider = provider;
            report.InstanceId = instanceId;
            report.StatusType = statusType;
            report.Metrics = status;
            report.Properties = properties;
            return report;
        }
    }
}