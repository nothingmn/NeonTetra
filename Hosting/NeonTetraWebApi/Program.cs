using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetraWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new DryIocServiceProviderFactory())
                .ConfigureContainer<Container>((hostContext, container) =>
                {
                    Deployment = InitializeApplication(container).Result;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static IDeployment Deployment { get; set; }

        private static async Task<IDeployment> InitializeApplication(IContainer container)
        {
            var diContainer = new NeonTetra.DI.DIContainer(container);
            var manager = new ConfigurationDeploymentManager();
            return await manager.Start(diContainer, new Dictionary<string, object>
            {
                {"host", typeof(Program).FullName},
                {"id", System.Guid.NewGuid().ToString()}
            });
        }
    }
}