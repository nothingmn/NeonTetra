using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using NeonTetra.Contracts;
using Newtonsoft.Json;

namespace NeonTetra.Services.Akka
{
    public class ActorSystemCore : IDisposable
    {
        private readonly string _name;
        public ActorSystem RootActorSystem { get; }

        public ActorSystemCore(string name = "MyActorSystem")
        {
            _name = name;
            var configManager = new ActorSystemConfiguration();
            var config = configManager.LoadHoconConfigPriority(_name);

            RootActorSystem = SetupActorSystem(_name, config);
        }

        private ActorSystem SetupActorSystem(string clusterName, Config config)
        {
            try
            {
                ActorSystem system = null;
                if (config == null)
                {
                    system = ActorSystem.Create(clusterName);
                }
                else
                {
                    system = ActorSystem.Create(clusterName, config);
                }

                return system;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task Terminate()
        {
            return RootActorSystem?.Terminate();
        }

        public void Dispose()
        {
            RootActorSystem?.Terminate();
            RootActorSystem?.Dispose();
        }
    }
}