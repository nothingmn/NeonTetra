using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeonTetra.Contracts;
using NeonTetra.Contracts.ActorSystem;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Infrastructure.Compression;
using NeonTetra.Contracts.Logging;

namespace NeonTetraConsole
{
    internal class Program
    {
        private static IDeployment Deployment { get; set; }

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Starting up...");

            Deployment = await InitializeApplication();
            var log = Deployment.Container.Resolve<ILogFactory>().CreateLog(typeof(Program));

            log.Information("Started.");
            var input = "";
            do
            {
                Console.Write(">");
                input = Console.ReadLine();
                Console.WriteLine("");

                switch (input)
                {
                    case "c":
                        var cFactory = Deployment.Container.Resolve<ICompressionFactory>();
                        var test = System.Guid.NewGuid().ToByteArray();
                        var compressed = cFactory.Compress(test, CompressionType.Gzip);
                        var uncompressed = cFactory.Decompress(compressed, CompressionType.Gzip);
                        log.Information("Compression Works:{0}", test.SequenceEqual(uncompressed));
                        break;

                    case "a":
                        var actorManager = Deployment.Container.Resolve<IActorManager>();
                        var loggingActor = actorManager.Create<ISimpleLoggingActor>("ISimpleLoggingActor1");
                        loggingActor.Tell("Hello world");

                        break;

                    default:
                        break;
                }
            } while (input != "q");
        }

        private static async Task<IDeployment> InitializeApplication()
        {
            var diContainer = new NeonTetra.DI.DIContainer();
            var manager = new ConfigurationDeploymentManager();
            return await manager.Start(diContainer, new Dictionary<string, object>
            {
                {"host", typeof(Program).FullName},
                {"id", System.Guid.NewGuid().ToString()}
            });
        }
    }
}