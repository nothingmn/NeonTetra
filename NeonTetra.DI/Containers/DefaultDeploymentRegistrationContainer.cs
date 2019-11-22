using System.Collections;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Services.Akka;
using NeonTetra.Services.MqttServer;
using NeonTetra.Services.SunriseSunset;

namespace NeonTetra.DI.Containers
{
    public class DefaultDeploymentRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public async Task ExecutePostRegistrationStep(IDIContainer container,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var metrics = new Dictionary<string, double>();
            var timer = new InlineEventTimer("DefaultDeployment", "InjectRegistrationModule");
            var postRegistrationSteps = container.ResolveAllInstances<IPostRegistrationStep>();

            if (postRegistrationSteps != null)
            {
                var orderedSteps = new SortedDictionary<int, IPostRegistrationStep>();
                var count = 1000;
                foreach (var s in from p in postRegistrationSteps where p.GetType() != GetType() select p)
                {
                    var step = s.GetType().GetCustomAttribute(typeof(RegistrationStepOrderAttribute));
                    if (step == null)
                    {
                        orderedSteps.Add(count, s);
                    }
                    else
                    {
                        var order = (step as RegistrationStepOrderAttribute)?.Order;
                        if (order.HasValue)
                            orderedSteps.Add(order.Value, s);
                        else
                            orderedSteps.Add(count, s);
                    }

                    count++;
                }

                foreach (var step in orderedSteps)
                    if (step.Value != null)
                    {
                        timer.Reset();
                        await step.Value.ExecutePostRegistrationStep(container, cancellationToken);
                        metrics.Add(step.Value.GetType().Name, timer.Elapsed.TotalMilliseconds);
                    }
            }

            container.Resolve<ILogFactory>()?.CreateLog("DefaultDeployment", "Start")?.Event("Startup Performance", null, metrics);
        }

        public void Register(IDIContainer container)
        {
            new CoreServicesRegistrationContainer().Register(container);
            new ConfigurationRegistrationContainer().Register(container);
            new LoggingRegistrationContainer().Register(container);
            new CacheRegistrationContainer().Register(container);
            new ResourceCompilerRegistrationContainer().Register(container);
            new AkkaRegistrationContainer().Register(container);
            new MqttBrokerRegistrationContainer().Register(container);
            new SunriseSunsetRegistrationContainer().Register(container);

            container.Register<IPostRegistrationStep, DefaultDeploymentRegistrationContainer>("DefaultDeploymentRegistrationContainer");
            container.Register<IPostRegistrationStep, LoggingRegistrationContainer>("LoggingRegistrationContainer");
            container.Register<IPostRegistrationStep, ConfigurationRegistrationContainer>("ConfigurationRegistrationContainer");
            container.Register<IPostRegistrationStep, CacheRegistrationContainer>("CacheRegistrationContainer");
            container.Register<IPostRegistrationStep, ResourceCompilerRegistrationContainer>("ResourceCompilerRegistrationContainer");
            container.Register<IPostRegistrationStep, AkkaRegistrationContainer>("AkkaRegistrationContainer");
            container.Register<IPostRegistrationStep, MqttBrokerRegistrationContainer>("MqttBrokerRegistrationContainer");
            container.Register<IPostRegistrationStep, SunriseSunsetRegistrationContainer>("SunriseSunsetRegistrationContainer");
        }
    }
}