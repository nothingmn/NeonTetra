using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Logging;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Serilog;
using Serilog.Core;
using Serilog.Events;

//using Serilog.ExtensionMethods;
using Serilog.Parsing;

namespace NeonTetra.Services.Serilog
{
    public class SerilogLogger : ILog
    {
        private static readonly Dictionary<LogEventLevel, SeverityLevel> AppInsightsSeverityLevelMap =
            new Dictionary<LogEventLevel, SeverityLevel>
            {
                {LogEventLevel.Verbose, SeverityLevel.Verbose},
                {LogEventLevel.Debug, SeverityLevel.Verbose},
                {LogEventLevel.Information, SeverityLevel.Information},
                {LogEventLevel.Warning, SeverityLevel.Warning},
                {LogEventLevel.Error, SeverityLevel.Error},
                {LogEventLevel.Fatal, SeverityLevel.Critical}
            };

        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, string> _contextProperties;

        private readonly Logger _logger;

        public SerilogLogger(IResolve resolver)
        {
            var loggerConfiguration = new LoggerConfiguration();

            if (resolver.IsRegistered<IConfiguration>())
            {
                _configuration = resolver.Resolve<IConfiguration>();

                if (_configuration.GetValue<bool>("Logging:Seq:Enabled"))
                {
                    var host = _configuration.GetValue<string>("Logging:Seq:Host");
                    if (!string.IsNullOrEmpty(host))
                    {
                        loggerConfiguration.WriteTo.Seq(host);
                    }
                }

                if (_configuration.DebugMode)
                {
                    loggerConfiguration.WriteTo.Debug();
                    WriteToDebug = true;
                }

                //if (resolver.IsRegistered<IAzureServicesConfiguration>())
                //{
                //    var azureConfig = resolver.Resolve<IAzureServicesConfiguration>();
                //    if (azureConfig != null && azureConfig.ApplicationInsights != null &&
                //        azureConfig.ApplicationInsights.Enabled &&
                //        !string.IsNullOrEmpty(azureConfig.ApplicationInsights.Key))
                //    {
                //        loggerConfiguration.WriteTo.ApplicationInsights(azureConfig.ApplicationInsights.Key,
                //            LogEventToTelemetryConverter);
                //        TelemetryDebugWriter.IsTracingDisabled = true;
                //        WriteToAppInsights = true;
                //    }
                //}

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    try
                    {
                        loggerConfiguration.WriteTo.ColoredConsole();
                        WriteToColoredConsole = true;
                    }
                    catch (Exception)
                    {
                        //optional, swallow if failes
                    }
                }

                var levelMap = new Dictionary<string, LogEventLevel>(StringComparer.InvariantCultureIgnoreCase)
                {
                    {"DEBUG", LogEventLevel.Debug},
                    {"INFO", LogEventLevel.Information},
                    {"EVENT", LogEventLevel.Information},
                    {"ERROR", LogEventLevel.Error},
                    {"FATAL", LogEventLevel.Fatal},
                    {"ALL", LogEventLevel.Verbose}
                };

                var levels = _configuration.GetValueOrDefault("Logging:Levels", "NONE");

                if (levelMap.TryGetValue(levels, out var logEventLevel))
                    loggerConfiguration.MinimumLevel.Is(logEventLevel);
            }

            _logger = loggerConfiguration.CreateLogger();
            _contextProperties = new ConcurrentDictionary<string, string>();
        }

        public bool WriteToDebug { get; }
        public bool WriteToColoredConsole { get; }
        public bool WriteToAppInsights { get; }

        public void Debug(string messageTemplate)
        {
            _logger.Debug(messageTemplate);
        }

        public void Debug(Exception exception, string messageTemplate)
        {
            _logger.Debug(exception, messageTemplate);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            _logger.Debug(messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Debug(exception, messageTemplate, propertyValues);
        }

        public void Debug<T>(string messageTemplate, T propertyValue)
        {
            _logger.Debug(messageTemplate, propertyValue);
        }

        public void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            _logger.Debug(exception, messageTemplate, propertyValue);
        }

        public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Debug(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            _logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error(string messageTemplate)
        {
            _logger.Error(messageTemplate);
        }

        public void Error(Exception exception, string messageTemplate)
        {
            _logger.Error(exception, messageTemplate);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            _logger.Error(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Error(exception, messageTemplate, propertyValues);
        }

        public void Error<T>(string messageTemplate, T propertyValue)
        {
            _logger.Error(messageTemplate, propertyValue);
        }

        public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            _logger.Error(exception, messageTemplate, propertyValue);
        }

        public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Error(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Error(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            _logger.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            _logger.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
            _logger.Fatal(exception, messageTemplate);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Fatal(exception, messageTemplate, propertyValues);
        }

        public void Fatal<T>(string messageTemplate, T propertyValue)
        {
            _logger.Fatal(messageTemplate, propertyValue);
        }

        public void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            _logger.Fatal(exception, messageTemplate, propertyValue);
        }

        public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Fatal(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            _logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information(string messageTemplate)
        {
            _logger.Information(messageTemplate);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            _logger.Information(messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate)
        {
            _logger.Information(exception, messageTemplate);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Information(exception, messageTemplate, propertyValues);
        }

        public void Information<T>(string messageTemplate, T propertyValue)
        {
            _logger.Information(messageTemplate, propertyValue);
        }

        public void Information<T>(Exception exception, string messageTemplate, T propertyValue)
        {
            _logger.Information(exception, messageTemplate, propertyValue);
        }

        public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Information(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0,
            T1 propertyValue1)
        {
            _logger.Information(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            _logger.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0,
            T1 propertyValue1,
            T2 propertyValue2)
        {
            _logger.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Event(string eventName, IDictionary<string, string> properties, IDictionary<string, double> metrics)
        {
            _logger.Information(eventName + " {properties};{metrics}", properties, metrics);
        }

        public void AddToContext(IDictionary<string, string> properties)
        {
            if (properties != null)
                foreach (var property in properties)
                    _contextProperties.TryAdd(property.Key, property.Value);
        }

        //private ITelemetry LogEventToTelemetryConverter(LogEvent logEvent, IFormatProvider formatProvider)
        //{
        //    // first create a default TraceTelemetry using the sink's default logic
        //    // .. but without the log level, and (rendered) message (template) included in the Properties

        //    ITelemetry telemetry;
        //    if (logEvent.MessageTemplate.Tokens.OfType<PropertyToken>().Any(t => t.PropertyName == "metrics"))
        //    {
        //        var evt = logEvent.ToDefaultEventTelemetry(
        //            formatProvider,
        //            true,
        //            false,
        //            false
        //        );

        //        evt.Name = ((TextToken)logEvent.MessageTemplate.Tokens.First()).Text;

        //        const string propertyKeyPrefix = "properties.";
        //        const string metricKeyPrefix = "metrics.";

        //        var propertyKeys = evt.Properties.Keys.Where(key => key.StartsWith(propertyKeyPrefix)).ToArray();
        //        var metricKeys = evt.Properties.Keys.Where(key => key.StartsWith(metricKeyPrefix)).ToArray();

        //        Array.ForEach(propertyKeys, key =>
        //        {
        //            var normalizedKey = key.Substring(propertyKeyPrefix.Length);
        //            evt.Properties[normalizedKey] = evt.Properties[key];
        //            evt.Properties.Remove(key);
        //        });

        //        Array.ForEach(metricKeys, key =>
        //        {
        //            if (double.TryParse(evt.Properties[key], out var value))
        //            {
        //                var normalizedKey = key.Substring(metricKeyPrefix.Length);
        //                evt.Metrics[normalizedKey] = value;
        //                evt.Properties.Remove(key);
        //            }
        //        });

        //        telemetry = evt;
        //    }
        //    else if (logEvent.Exception != null)
        //    {
        //        var ex = logEvent.ToDefaultExceptionTelemetry(
        //            formatProvider,
        //            true,
        //            true,
        //            false
        //        );
        //        ex.Message = ex.Exception.Message;

        //        if (AppInsightsSeverityLevelMap.TryGetValue(logEvent.Level, out var severityLevel))
        //            ex.SeverityLevel = severityLevel;

        //        telemetry = ex;
        //    }
        //    else
        //    {
        //        var trc = logEvent.ToDefaultTraceTelemetry(
        //            formatProvider,
        //            true,
        //            false,
        //            false
        //        );
        //        trc.Message = logEvent.RenderMessage(formatProvider);
        //        if (AppInsightsSeverityLevelMap.TryGetValue(logEvent.Level, out var severityLevel))
        //            trc.SeverityLevel = severityLevel;

        //        telemetry = trc;
        //    }

        //    foreach (var contextProperty in _contextProperties)
        //        TryAddProperty(telemetry, contextProperty.Key, contextProperty.Value);

        //    telemetry.Context.Component.Version = _configuration?.Version;

        //    return telemetry;
        //}

        private static void TryAddProperty(ITelemetry telemetry, string name, string value)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                if (!telemetry.Context.Properties.ContainsKey(name))
                    telemetry.Context.Properties.Add(name, value);
        }
    }
}