using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.DI.Core;
using NeonTetra.Contracts;
using NeonTetra.Contracts.ActorSystem;

namespace NeonTetra.Services.Akka
{
    public class NeonTetraDIBridge : IDependencyResolver
    {
        private readonly IDIContainer _container;
        private readonly ActorSystemCore _systemCore;

        public NeonTetraDIBridge(IDIContainer container, ActorSystemCore systemCore)
        {
            _container = container;
            _systemCore = systemCore;
        }

        public Type GetType(string actorName)
        {
            if (_container.IsRegistered<IActorRef>(actorName))
            {
                return _container.Resolve<IActorRef>(actorName)?.GetType();
            }
            return null;
        }

        public Func<ActorBase> CreateActorFactory(Type actorType)
        {
            return () => (ActorBase)_container.Resolve(actorType);
        }

        public Props Create<TActor>() where TActor : ActorBase
        {
            return _systemCore.RootActorSystem.GetExtension<DIExt>().Props(typeof(TActor));
        }

        public Props Create(Type actorType)
        {
            return _systemCore.RootActorSystem.GetExtension<DIExt>().Props(actorType);
        }

        public void Release(ActorBase actor)
        {
        }
    }
}