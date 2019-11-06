using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NeonTetra.Contracts.Logging;
using NeonTetra.Contracts.Serialization;

namespace NeonTetra.Core.Logging
{
    public class ConsoleLogger : ILog
    {
        private readonly ISerializer _serializer;

        private IDictionary<string, string> _contextProperties;

        public ConsoleLogger(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void Debug(string messageTemplate)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "DEBUG",
                messageTemplate,
                _contextProperties
            }));
        }

        public void Debug(Exception exception, string messageTemplate)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "DEBUG",
                exception,
                messageTemplate,
                _contextProperties
            }));
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "DEBUG",
                messageTemplate,
                propertyValues,
                _contextProperties
            }));
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "DEBUG",
                exception,
                messageTemplate,
                propertyValues,
                _contextProperties
            }));
        }

        public void Debug<T>(string messageTemplate, T propertyValue)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "DEBUG",
                messageTemplate,
                propertyValue,
                _contextProperties
            }));
        }

        public void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "DEBUG",
                exception,
                messageTemplate,
                propertyValue,
                _contextProperties
            }));
        }

        public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "DEBUG",
                messageTemplate,
                propertyValue0,
                propertyValue1,
                _contextProperties
            }));
        }

        public void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "DEBUG",
                exception,
                messageTemplate,
                propertyValue0,
                propertyValue1,
                _contextProperties
            }));
        }

        public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "DEBUG",
                messageTemplate,
                propertyValue0,
                propertyValue1,
                propertyValue2,
                _contextProperties
            }));
        }

        public void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "DEBUG",
                exception,
                messageTemplate,
                propertyValue0,
                propertyValue1,
                propertyValue2,
                _contextProperties
            }));
        }

        public void Error(string messageTemplate)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "ERROR",
                messageTemplate,
                _contextProperties
            }));
        }

        public void Error(Exception exception, string messageTemplate)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "ERROR",
                exception,
                messageTemplate,
                _contextProperties
            }));
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "ERROR",
                messageTemplate,
                propertyValues,
                _contextProperties
            }));
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "ERROR",
                exception,
                messageTemplate,
                propertyValues,
                _contextProperties
            }));
        }

        public void Error<T>(string messageTemplate, T propertyValue)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "ERROR",
                messageTemplate,
                propertyValue,
                _contextProperties
            }));
        }

        public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "ERROR",
                exception,
                messageTemplate,
                propertyValue,
                _contextProperties
            }));
        }

        public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "ERROR",
                messageTemplate,
                propertyValue0,
                propertyValue1,
                _contextProperties
            }));
        }

        public void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "ERROR",
                exception,
                messageTemplate,
                propertyValue0,
                propertyValue1,
                _contextProperties
            }));
        }

        public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "ERROR",
                messageTemplate,
                propertyValue0,
                propertyValue1,
                propertyValue2,
                _contextProperties
            }));
        }

        public void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "ERROR",
                exception,
                messageTemplate,
                propertyValue0,
                propertyValue1,
                propertyValue2,
                _contextProperties
            }));
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "FATAL",
                exception,
                messageTemplate,
                _contextProperties
            }));
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "FATAL",
                messageTemplate,
                propertyValues,
                _contextProperties
            }));
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "FATAL",
                exception,
                messageTemplate,
                propertyValues,
                _contextProperties
            }));
        }

        public void Fatal<T>(string messageTemplate, T propertyValue)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "FATAL",
                messageTemplate,
                propertyValue,
                _contextProperties
            }));
        }

        public void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "FATAL",
                exception,
                messageTemplate,
                propertyValue,
                _contextProperties
            }));
        }

        public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "FATAL",
                messageTemplate,
                propertyValue0,
                propertyValue1,
                _contextProperties
            }));
        }

        public void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "FATAL",
                exception,
                messageTemplate,
                propertyValue0,
                propertyValue1,
                _contextProperties
            }));
        }

        public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "FATAL",
                messageTemplate,
                propertyValue0,
                propertyValue1,
                propertyValue2,
                _contextProperties
            }));
        }

        public void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "FATAL",
                exception,
                messageTemplate,
                propertyValue0,
                propertyValue1,
                propertyValue2,
                _contextProperties
            }));
        }

        public void Information(string messageTemplate)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                messageTemplate,
                _contextProperties
            }));
        }

        public void Information(Exception exception, string messageTemplate)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                exception,
                messageTemplate,
                _contextProperties
            }));
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                messageTemplate,
                propertyValues,
                _contextProperties
            }));
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                exception,
                messageTemplate,
                propertyValues,
                _contextProperties
            }));
        }

        public void Information<T>(string messageTemplate, T propertyValue)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                messageTemplate,
                propertyValue,
                _contextProperties
            }));
        }

        public void Information<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                exception,
                messageTemplate,
                propertyValue,
                _contextProperties
            }));
        }

        public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                messageTemplate,
                propertyValue0,
                propertyValue1,
                _contextProperties
            }));
        }

        public void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0,
            T1 propertyValue1)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                exception,
                messageTemplate,
                propertyValue0,
                propertyValue1,
                _contextProperties
            }));
        }

        public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                messageTemplate,
                propertyValue0,
                propertyValue1,
                propertyValue2,
                _contextProperties
            }));
        }

        public void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0,
            T1 propertyValue1, T2 propertyValue2)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                exception,
                messageTemplate,
                propertyValue0,
                propertyValue1,
                propertyValue2,
                _contextProperties
            }));
        }

        public void Event(string eventName, IDictionary<string, string> properties, IDictionary<string, double> metrics)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "INFORMATION",
                properties,
                metrics,
                _contextProperties
            }));
        }

        public void AddToContext(IDictionary<string, string> properties)
        {
            if (properties != null)
            {
                if (_contextProperties == null) _contextProperties = new ConcurrentDictionary<string, string>();
                properties.ToList().ForEach(x =>
                {
                    if (!_contextProperties.ContainsKey(x.Key)) _contextProperties.Add(x.Key, x.Value);
                });
            }
        }

        public void Fatal(string messageTemplate)
        {
            Console.WriteLine(_serializer.SerializeToString(new
            {
                DateTimeOffset.UtcNow,
                Level = "FATAL",
                messageTemplate,
                _contextProperties
            }));
        }
    }
}