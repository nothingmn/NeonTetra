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

        public AkkaActorManager(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }

        public async Task<INeonActor> GetByPath<T>(string path)
        {
            var sel = _actorSystem.ActorSelection(path);
            if (sel != null)
            {
                try
                {
                    var x = _actorSystem.ActorOf(_actorSystem.DI().Props(typeof(T)), path);
                    var actorRef = await sel.ResolveOne(TimeSpan.FromSeconds(5));
                    if (actorRef != null)
                    {
                        return new NeonActorWrapper(actorRef);
                    }
                }
                catch
                {
                    //actor was not found, ignore
                }
            }

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

        public object ActorReference
        {
            get { return ActorRef; }
        }

        public NeonActorWrapper(IActorRef actorRef)
        {
            ActorRef = actorRef;
        }

        public void Forward(object message)
        {
            ActorRef.Forward(message);
        }

        public void Tell(object message, INeonActor sender = null)
        {
            if (sender != null && sender.ActorReference != null)
            {
                ActorRef.Tell(message, sender.ActorReference as IActorRef);
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