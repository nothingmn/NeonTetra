using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.DI.Core;
using NeonTetra.Contracts;
using NeonTetra.Contracts.ActorSystem;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Services.Akka.Actors;

namespace NeonTetra.Services.Akka
{
    public class AkkaRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public void Register(IDIContainer container)
        {
            container.Register<NeonActorWrapper, NeonActorWrapper>();
            container.Register<SimpleLoggingActor, SimpleLoggingActor>();
            container.Register<ISimpleLoggingActor, SimpleLoggingActor>();
            container.Register<IEchoActor, EchoActor>();
            container.Register<IUserActor, UserActor>();
            container.Register<IActorManager, AkkaActorManager>();
        }

        public Task ExecutePostRegistrationStep(IDIContainer container, CancellationToken cancellationToken = default(CancellationToken))
        {
            var config = container.Resolve<IConfiguration>();
            var actorSystemName = config.GetValueOrDefault("Actor:System:Name", "NeonTetra");
            var actorCore = new ActorSystemCore(container, actorSystemName);
            var resolver = new NeonTetraDIBridge(container, actorCore);
            actorCore.RootActorSystem.AddDependencyResolver(resolver);

            container.RegisterInstance(resolver, typeof(IDependencyResolver));
            container.RegisterInstance(actorCore);
            container.RegisterInstance(actorCore.RootActorSystem);
            return Task.CompletedTask;
        }
    }
}