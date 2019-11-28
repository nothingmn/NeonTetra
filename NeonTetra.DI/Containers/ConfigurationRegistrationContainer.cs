using System.Collections.Generic;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Core.Configuration;
using System.Threading;
using System.Threading.Tasks;
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

            if (!System.IO.Directory.Exists(appData.FullName)) System.IO.Directory.CreateDirectory(appData.FullName);

            await LoadUsersAsync(container, config);
        }

        private async Task LoadUsersAsync(IDIContainer container, IConfiguration configuration)
        {
            var accountManager = container.Resolve<IAccountManager>();
            var serializer = container.Resolve<IDeserialize>();
            var usersConfigFolder = System.IO.Path.Combine(configuration.AppDataDirectory.FullName, "Users");

            if (!System.IO.Directory.Exists(usersConfigFolder)) System.IO.Directory.CreateDirectory(usersConfigFolder);

            foreach (var userConfig in System.IO.Directory.GetFiles(usersConfigFolder))
            {
                if (System.IO.File.Exists(userConfig))
                {
                    var userJson = System.IO.File.ReadAllText(userConfig);
                    if (!string.IsNullOrEmpty(userJson))
                    {
                        var users = serializer.Deserialize<List<IUser>>(userJson);
                        if (users != null && users.Count > 0)
                        {
                            foreach (var user in users)
                            {
                                await accountManager.RegisterUser(user);
                            }
                        }
                    }
                }
            }
        }
    }
}