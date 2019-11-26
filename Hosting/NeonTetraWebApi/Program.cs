using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Hangfire;
using Hangfire.MySql.Core;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Core.Configuration;
using NeonTetraWebApi.Scheduling;

namespace NeonTetraWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var iocProviderFatory = new DryIocServiceProviderFactory();

            Startup.JobActivator = new NeonTetraJobActivator();
            var hostBuilder = Host.CreateDefaultBuilder(args)
                    .UseServiceProviderFactory(iocProviderFatory)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        var webHostBuilder = webBuilder.UseStartup<Startup>();
                    })
                    .ConfigureContainer<Container>((hostContext, container) =>
                    {
                        Startup.Deployment = InitializeApplication(container).Result;
                        (Startup.JobActivator as NeonTetraJobActivator).Resolver = Startup.Deployment.Container;
                    })
                ;
            return hostBuilder;
        }

        private static async Task<IDeployment> InitializeApplication(IContainer container)
        {
            var diContainer = new NeonTetra.DI.DIContainer(container);
            var manager = new ConfigurationDeploymentManager();
            diContainer.Register<NeonTetraJobActivator, NeonTetraJobActivator>();
            diContainer.InjectRegistrationModule(typeof(WebRegistrationContainer));
            return await manager.Start(diContainer, new Dictionary<string, object>
            {
                {"host", typeof(Program).FullName},
                {"id", System.Guid.NewGuid().ToString()}
            });
        }
    }
}