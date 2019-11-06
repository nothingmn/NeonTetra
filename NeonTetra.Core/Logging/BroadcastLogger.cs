using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Core.Logging
{
    public class BroadcastLogger : ILog
    {
        private readonly bool _enabled;
        private readonly ImmutableArray<ILog> _loggers;

        private readonly bool debugEnabled;
        private readonly bool errorEnabled;
        private readonly bool eventEnabled;
        private readonly bool fatalEnabled;
        private readonly bool informationEnabled;

        public BroadcastLogger(IEnumerable<ILog> logs, IResolve resolver)
        {
            _loggers = (from l in logs where l.GetType() != typeof(BroadcastLogger) select l).ToImmutableArray();
            _enabled = _loggers.Any();

            if (resolver.IsRegistered<IConfiguration>())
            {
                var config = resolver.Resolve<IConfiguration>();
                if (config.IsReady)
                {
                    var levels = config.GetValueOrDefault("Logging:Levels", "NONE");
                    if (levels.IndexOf("DEBUG", StringComparison.InvariantCultureIgnoreCase) >= 0) debugEnabled = true;
                    if (levels.IndexOf("INFO", StringComparison.InvariantCultureIgnoreCase) >= 0)
                        informationEnabled = true;
                    if (levels.IndexOf("EVENT", StringComparison.InvariantCultureIgnoreCase) >= 0) eventEnabled = true;
                    if (levels.IndexOf("ERROR", StringComparison.InvariantCultureIgnoreCase) >= 0) errorEnabled = true;
                    if (levels.IndexOf("FATAL", StringComparison.InvariantCultureIgnoreCase) >= 0) fatalEnabled = true;
                    if (levels.IndexOf("ALL", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        debugEnabled = true;
                        informationEnabled = true;
                        eventEnabled = true;
                        errorEnabled = true;
                        fatalEnabled = true;
                    }
                }
            }
        }

        public void Debug(string messageTemplate)
        {
            if (!_enabled || !debugEnabled) return;
            foreach (var l in _loggers) l.Debug(messageTemplate);
        }

        public void Debug(Exception exception, string messageTemplate)
        {
            if (!_enabled || !debugEnabled) return;
            foreach (var l in _loggers) l.Debug(exception, messageTemplate);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            if (!_enabled || !debugEnabled) return;
            foreach (var l in _loggers) l.Debug(messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!_enabled || !debugEnabled) return;
            foreach (var l in _loggers) l.Debug(exception, messageTemplate, propertyValues);
        }

        public void Debug<T>(string messageTemplate, T propertyValue)
        {
            if (!_enabled || !debugEnabled) return;
            foreach (var l in _loggers) l.Debug(messageTemplate, propertyValue);
        }

        public void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            if (!_enabled || !debugEnabled) return;
            foreach (var l in _loggers) l.Debug(exception, messageTemplate, propertyValue);
        }

        public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (!_enabled || !debugEnabled) return;
            foreach (var l in _loggers) l.Debug(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (!_enabled || !debugEnabled) return;
            foreach (var l in _loggers) l.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            if (!_enabled || !debugEnabled) return;
            foreach (var l in _loggers) l.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            if (!_enabled || !debugEnabled) return;
            foreach (var l in _loggers)
                l.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error(string messageTemplate)
        {
            if (!_enabled || !errorEnabled) return;
            foreach (var l in _loggers) l.Error(messageTemplate);
        }

        public void Error(Exception exception, string messageTemplate)
        {
            if (!_enabled || !errorEnabled) return;
            foreach (var l in _loggers) l.Error(exception, messageTemplate);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            if (!_enabled || !errorEnabled) return;
            foreach (var l in _loggers) l.Error(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!_enabled || !errorEnabled) return;
            foreach (var l in _loggers) l.Error(exception, messageTemplate, propertyValues);
        }

        public void Error<T>(string messageTemplate, T propertyValue)
        {
            if (!_enabled || !errorEnabled) return;
            foreach (var l in _loggers) l.Error(messageTemplate, propertyValue);
        }

        public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            if (!_enabled || !errorEnabled) return;
            foreach (var l in _loggers) l.Error(exception, messageTemplate, propertyValue);
        }

        public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (!_enabled || !errorEnabled) return;
            foreach (var l in _loggers) l.Error(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (!_enabled || !errorEnabled) return;
            foreach (var l in _loggers) l.Error(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            if (!_enabled || !errorEnabled) return;
            foreach (var l in _loggers) l.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            if (!_enabled || !errorEnabled) return;
            foreach (var l in _loggers)
                l.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            if (!_enabled || !fatalEnabled) return;
            foreach (var l in _loggers) l.Error(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
            if (!_enabled || !fatalEnabled) return;
            foreach (var l in _loggers) l.Fatal(exception, messageTemplate);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!_enabled || !fatalEnabled) return;
            foreach (var l in _loggers) l.Fatal(exception, messageTemplate, propertyValues);
        }

        public void Fatal<T>(string messageTemplate, T propertyValue)
        {
            if (!_enabled || !fatalEnabled) return;
            foreach (var l in _loggers) l.Fatal(messageTemplate, propertyValue);
        }

        public void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            if (!_enabled || !fatalEnabled) return;
            foreach (var l in _loggers) l.Fatal(exception, messageTemplate, propertyValue);
        }

        public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (!_enabled || !fatalEnabled) return;
            foreach (var l in _loggers) l.Fatal(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (!_enabled || !fatalEnabled) return;
            foreach (var l in _loggers) l.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            if (!_enabled || !fatalEnabled) return;
            foreach (var l in _loggers) l.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            if (!_enabled || !fatalEnabled) return;
            foreach (var l in _loggers)
                l.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information(string messageTemplate)
        {
            if (!_enabled || !informationEnabled) return;
            foreach (var l in _loggers) l.Information(messageTemplate);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            if (!_enabled || !informationEnabled) return;
            foreach (var l in _loggers) l.Information(messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate)
        {
            if (!_enabled || !informationEnabled) return;
            foreach (var l in _loggers) l.Information(exception, messageTemplate);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!_enabled || !informationEnabled) return;
            foreach (var l in _loggers) l.Information(exception, messageTemplate, propertyValues);
        }

        public void Information<T>(string messageTemplate, T propertyValue)
        {
            if (!_enabled || !informationEnabled) return;
            foreach (var l in _loggers) l.Information(messageTemplate, propertyValue);
        }

        public void Information<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            if (!_enabled || !informationEnabled) return;
            foreach (var l in _loggers) l.Information(exception, messageTemplate, propertyValue);
        }

        public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (!_enabled || !informationEnabled) return;
            foreach (var l in _loggers) l.Information(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0,
            T1 propertyValue1)
        {
            if (!_enabled || !informationEnabled) return;
            foreach (var l in _loggers) l.Information(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            if (!_enabled || !informationEnabled) return;
            foreach (var l in _loggers) l.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0,
            T1 propertyValue1,
            T2 propertyValue2)
        {
            if (!_enabled || !informationEnabled) return;
            foreach (var l in _loggers)
                l.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Event(string eventName, IDictionary<string, string> properties, IDictionary<string, double> metrics)
        {
            if (!_enabled || !eventEnabled) return;
            foreach (var l in _loggers) l.Event(eventName, properties, metrics);
        }

        public void AddToContext(IDictionary<string, string> properties)
        {
            foreach (var l in _loggers) l.AddToContext(properties);
        }
    }
}