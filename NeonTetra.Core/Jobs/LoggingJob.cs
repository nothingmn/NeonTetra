using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Jobs;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Core.Jobs
{
    public class LoggingJob : ILoggingJob
    {
        private readonly ILogFactory _logFactory;

        public LoggingJob(ILogFactory logFactory)
        {
            _logFactory = logFactory;
        }

        public void Information(Type parentType, string message)
        {
            var log = _logFactory.CreateLog(parentType);
            log.Information(message);
        }
    }
}