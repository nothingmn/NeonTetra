using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Contracts
{
    public class ConfigurationDeploymentManager : IDeploymentManager
    {
        private readonly ConcurrentDictionary<string, IDeployment> _activeDeployments =
            new ConcurrentDictionary<string, IDeployment>();

        public async Task<IDeployment> Start(IDIContainer rootContainer, IDictionary<string, object> environment = null)
        {
            var id = Guid.NewGuid().ToString();

            if (environment != null)
                if (environment.ContainsKey("id"))
                    id = environment["id"].ToString();

            rootContainer.RegisterInstance(rootContainer);
            rootContainer.RegisterInstance(rootContainer as IRegister);
            rootContainer.RegisterInstance(rootContainer as IResolve);

            if (environment != null)
            {
                rootContainer.RegisterInstance(environment, "environment");
                foreach (var key in environment.Keys)
                    rootContainer.RegisterInstance(environment[key], "environment." + key);
            }

            IDeployment deployment = null;
            if (environment != null && environment.ContainsKey("Deployment"))
            {
                deployment = environment["Deployment"] as IDeployment;
            }
            else if (rootContainer.IsRegistered<IConfiguration>())
            {
                var config = rootContainer.Resolve<IConfiguration>();
                var deployConfig = config.GetValue<string>("Deployment");
                if (!string.IsNullOrEmpty(deployConfig))
                {
                    var type = rootContainer.GetTypeByModuleName(deployConfig);
                    if (type != null) deployment = type.Assembly.CreateInstance(type.FullName, false) as IDeployment;
                }
            }

            if (deployment == null) deployment = new DefaultDeployment();

            await deployment.Start(rootContainer, environment);

            while (!_activeDeployments.TryAdd(id, deployment)) await Task.Delay(1);
            rootContainer.RegisterInstance(deployment);

            Started = true;
            return deployment;
        }

        public bool Started { get; private set; }
    }
}