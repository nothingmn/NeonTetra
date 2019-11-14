using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using NeonTetra.Contracts;
using NeonTetra.Contracts.ActorSystem;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.ActorSystem.Messages;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Membership;
using NeonTetra.Services.Akka.Actors;

namespace NeonTetra.Services.Akka
{
    public class AkkaRegistrationContainer : IRegistrationContainer, IPostRegistrationStep
    {
        public void Register(IDIContainer container)
        {
            container.Register<IUserActor, UserActor>();
            container.Register<IUserManager, AkkaUserManager>();
            container.RegisterSingleton<IUserManagerActor, UserManagerActor>();
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

            var _userManagerActorRef = actorCore.RootActorSystem.ActorOf(actorCore.RootActorSystem.DI().Props(typeof(IUserManagerActor)), "UserManagerActor");
            container.RegisterInstance(_userManagerActorRef, "UserManagerActor");

            return Task.CompletedTask;
        }
    }
}