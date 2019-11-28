using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Core.Configuration;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts.Logging;
using NeonTetra.Contracts.Membership;
using NeonTetra.Contracts.Serialization;

namespace NeonTetra.DI.Containers
{
    //[RegistrationStepOrder(Order = 1)]
    public class ConfigurationRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public void Register(IDIContainer container)
        {
            if (!container.IsRegistered<IConfiguration>())
            {
                container.Register<IConfiguration, Configuration>();
                var config = container.Resolve<IConfiguration>();
                config.Build();
                container.RegisterInstance(config, typeof(IConfiguration));
            }
        }

        public async Task ExecutePostRegistrationStep(IDIContainer container, CancellationToken cancellationToken = default(CancellationToken))
        {
            var config = container.Resolve<IConfiguration>();
            var appData = config.AppDataDirectory;
            var log = container.Resolve<ILogFactory>().CreateLog(this.GetType());

            if (!System.IO.Directory.Exists(appData.FullName)) System.IO.Directory.CreateDirectory(appData.FullName);

            await LoadUsersAsync(log, container, config);
        }

        private async Task LoadUsersAsync(ILog log, IDIContainer container, IConfiguration configuration)
        {
            var accountManager = container.Resolve<IAccountManager>();
            var serializer = container.Resolve<IDeserialize>();
            await LoadConfigForType<List<IUser>>(log, serializer, configuration, "Users", async users =>
            {
                if (users != default(List<IUser>))
                {
                    foreach (var user in users)
                    {
                        await accountManager.RegisterUser(user);
                    }
                }
            });
        }

        private async Task LoadConfigForType<T>(ILog log, IDeserialize serializer, IConfiguration configuration, string appDataSubFolderName, Func<T, Task> foundItems)
        {
            if (foundItems == null) return;
            var configFolder = System.IO.Path.Combine(configuration.AppDataDirectory.FullName, appDataSubFolderName);

            if (!System.IO.Directory.Exists(configFolder)) System.IO.Directory.CreateDirectory(configFolder);

            foreach (var file in System.IO.Directory.GetFiles(configFolder))
            {
                try
                {
                    if (System.IO.File.Exists(file))
                    {
                        var json = System.IO.File.ReadAllText(file);
                        if (!string.IsNullOrEmpty(json))
                        {
                            var items = serializer.Deserialize<T>(json);
                            if (items != null) await foundItems(items);
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Error(e, "Could not load config from file:{0}", file);
                }
            }
        }
    }
}