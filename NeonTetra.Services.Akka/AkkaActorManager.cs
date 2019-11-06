using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Actor.Dsl;
using Akka.DI.Core;
using NeonTetra.Contracts;
using NeonTetra.Contracts.ActorSystem;

namespace NeonTetra.Services.Akka
{
    public class AkkaActorManager : IActorManager
    {
        private readonly ActorSystem _actorSystem;
        private readonly IDIContainer _container;

        public AkkaActorManager(ActorSystem actorSystem, IDIContainer container)
        {
            _actorSystem = actorSystem;
            _container = container;
        }

        public INeonActor GetByPath(string path)
        {
            return null;
        }

        public INeonActor Create<T>(string id)
        {
            var actor = _actorSystem.ActorOf(_actorSystem.DI().Props(typeof(T)), id);
            return new NeonActorWrapper(actor);
        }
    }

    public class NeonActorWrapper : INeonActor
    {
        public IActorRef ActorRef { get; }

        public object RootActor
        {
            get { return ActorRef; }
        }

        public NeonActorWrapper(IActorRef actorRef)
        {
            ActorRef = actorRef;
        }

        public void Tell(object message, INeonActor sender = null)
        {
            if (sender != null && sender.RootActor != null)
            {
                ActorRef.Tell(message, sender.RootActor as IActorRef);
            }
            else
            {
                ActorRef.Tell(message);
            }
        }

        public async Task<T> Ask<T>(object message, TimeSpan? timeout = null)
        {
            return await ActorRef.Ask<T>(message, timeout);
        }
    }
}