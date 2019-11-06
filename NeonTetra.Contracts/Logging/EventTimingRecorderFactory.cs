using System;
using System.Runtime.CompilerServices;

namespace NeonTetra.Contracts.Logging
{
    public class EventTimingRecorderFactory : IEventTimingFactory
    {
        private readonly ILogFactory _logFactory;

        public EventTimingRecorderFactory(ILogFactory logFactory)
        {
            _logFactory = logFactory;
        }

        public IDisposable EventTimer(ILog log, string category, [CallerMemberName] string method = null,
            bool nanoSecondPrecision = false)
        {
            if (log == null) log = _logFactory.CreateLog(GetType());
            return new EventTimingRecorder(log, category, method, nanoSecondPrecision);
        }

        public IDisposable EventTimer(string category, [CallerMemberName] string method = null)
        {
            return new EventTimingRecorder(_logFactory.CreateLog(GetType()), category, method);
        }

        public IInlineEventTimer InlineEventTimer(string category, [CallerMemberName] string method = null)
        {
            return new InlineEventTimer(category, method);
        }

        public IAggregateInlineEventTimer AggregateInlineEventTimer()
        {
            return new AggregateInlineEventTimer();
        }
    }
}