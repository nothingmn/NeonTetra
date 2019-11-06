using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Logging;
using NeonTetra.Core.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Services.Serilog;

namespace NeonTetra.DI.Containers
{
    [RegistrationStepOrder(Order = 2)]
    public class LoggingRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public Task ExecutePostRegistrationStep(IDIContainer container,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Dictionary<string, string> loggingContext;

            if (container.IsRegistered<Dictionary<string, string>>("LoggingContext"))
            {
                loggingContext = container.Resolve<Dictionary<string, string>>("LoggingContext");
            }
            else
            {
                loggingContext = new Dictionary<string, string>();
                container.RegisterInstance(loggingContext, "LoggingContext");
            }

            loggingContext["Command Line"] = Environment.CommandLine;
            loggingContext["Current Directory"] = Environment.CurrentDirectory;
            loggingContext["User Domain Name"] = Environment.UserDomainName;
            loggingContext["User Name"] = Environment.UserName;
            loggingContext["OS Version"] = Environment.OSVersion.ToString();
            loggingContext["Machine Name"] = Environment.MachineName;

            var host = Environment.GetEnvironmentVariable("Host");
            loggingContext["Host"] = host;

            if (container.IsRegistered<IConfiguration>())
            {
                var config = container.Resolve<IConfiguration>();
                loggingContext["Host Environment"] = config.HostEnvironment;
                loggingContext["Entry Point"] = config.EntryPoint;
                loggingContext["Environment"] = config.Environment;
                loggingContext["Version"] = config.Version;
                loggingContext["App Data Directory"] = config.AppDataDirectory.ToString();
                loggingContext["Debug Mode"] = config.DebugMode.ToString().ToLowerInvariant();
                loggingContext["Root Directory"] = config.RootDirectory.ToString();
            }

            return Task.CompletedTask;
        }

        public void Register(IDIContainer container)
        {
            container.Register<ILogFactory, InjectedLogFactory>();
            container.Register<ILog, BroadcastLogger>();
            container.Register<ILog, SerilogLogger>(typeof(SerilogLogger).FullName);

            container.Register<IEventTimingFactory, EventTimingRecorderFactory>();
            container.Register<IInlineEventTimer, InlineEventTimer>();
            container.Register<IInstrumentationEntity, InstrumentationEntity>();
            container.Register<ICaptureAndAggregate, TimerCaptureAndAggregate>();
            container.Register<IMetricAggregator, MetricAggregator>();
        }
    }
}