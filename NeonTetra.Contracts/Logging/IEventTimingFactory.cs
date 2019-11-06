using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NeonTetra.Contracts.Logging
{
    public interface IEventTimingFactory
    {
        IDisposable EventTimer(ILog log, string category, [CallerMemberName] string method = null,
            bool nanoSecondPrecision = false);

        IDisposable EventTimer(string category, [CallerMemberName] string method = null);

        IInlineEventTimer InlineEventTimer(string category = null, [CallerMemberName] string method = null);

        IAggregateInlineEventTimer AggregateInlineEventTimer();
    }

    public interface IAggregateInlineEventTimer
    {
        double MemoryConsumption { get; }
        void RecordEvent(string eventName, double elapsed);

        void RecordEvent(string eventName, IInlineEventTimer eventTimer, bool reset = true);

        IList<IInlineAggregate> Summations();

        void Reset();
    }

    public class AggregateInlineEventTimer : IAggregateInlineEventTimer
    {
        private ConcurrentDictionary<string, ConcurrentBag<double>> _timers =
            new ConcurrentDictionary<string, ConcurrentBag<double>>();

        private double startingWorkingSet = Environment.WorkingSet;

        public double MemoryConsumption => Environment.WorkingSet - startingWorkingSet;

        public void RecordEvent(string eventName, double elapsed)
        {
            if (_timers.ContainsKey(eventName))
                _timers[eventName].Add(elapsed);
            else
                _timers.TryAdd(eventName, new ConcurrentBag<double>(new List<double> {elapsed}));
        }

        public void RecordEvent(string eventName, IInlineEventTimer eventTimer, bool reset = true)
        {
            var elapsed = eventTimer.Elapsed.TotalMilliseconds;

            if (_timers.ContainsKey(eventName))
                _timers[eventName].Add(elapsed);
            else
                _timers.TryAdd(eventName, new ConcurrentBag<double>(new List<double> {elapsed}));
            if (reset) eventTimer.Reset();
        }

        public IList<IInlineAggregate> Summations()
        {
            return (from t in _timers select new InlineAggregate {EventName = t.Key, Value = t.Value.Sum()})
                .OrderBy(a => a.Value).ToList<IInlineAggregate>();
        }

        public void Reset()
        {
            _timers = new ConcurrentDictionary<string, ConcurrentBag<double>>();
            startingWorkingSet = Environment.WorkingSet;
        }
    }

    public class InlineAggregate : IInlineAggregate
    {
        public string EventName { get; set; }
        public double Value { get; set; }
    }

    public interface IInlineAggregate
    {
        string EventName { get; set; }
        double Value { get; set; }
    }
}