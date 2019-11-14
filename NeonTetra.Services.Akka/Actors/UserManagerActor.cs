using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using NeonTetra.Contracts.ActorSystem;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.ActorSystem.Messages.Commands;
using NeonTetra.Contracts.Logging;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Services.Akka.Actors
{
    /// <summary>
    ///https://getakka.net/articles/intro/tutorial-3.html
    /// Responsible for:
    ///     Registering new users
    ///     Monitoring all active Users
    ///     Fail strategy for Users
    ///     Querying groups of users
    ///     Retreiving a single User actor
    /// </summary>
    public class UserManagerActor : ReceiveActor, IUserManagerActor
    {
        private readonly Dictionary<string, IActorRef> userIdToActor = new Dictionary<string, IActorRef>();
        private readonly Dictionary<IActorRef, string> actorToUserId = new Dictionary<IActorRef, string>();
        private readonly ILog _log;

        public UserManagerActor(ILogFactory logFactory)
        {
            _log = logFactory.CreateLog(GetType());

            Receive<IRequestTrackUserCommand>(msg =>
            {
                if (userIdToActor.TryGetValue(msg.UserId, out var actorRef))
                {
                    actorRef.Forward(msg);
                }
                else
                {
                    _log.Information("Creating User actor for {0}", msg.UserId);
                    var actor = Context.ActorOf(Context.DI().Props(typeof(IUserActor)), msg.UserId);
                    Context.Watch(actor);
                    userIdToActor.Add(msg.UserId, actor);
                    actorToUserId.Add(actor, msg.UserId);
                    actor.Forward(msg);
                }
            });
            Receive<Terminated>(t =>
            {
                var userId = actorToUserId[t.ActorRef];
                _log.Information("Terminating user actor for {0}", userId);
                actorToUserId.Remove(t.ActorRef);
                userIdToActor.Remove(userId);
            });
        }

        protected override void PreStart() => _log.Information("UserManagerActor started");

        protected override void PostStop() => _log.Information("UserManagerActor stopped");

        public object ActorReference { get; }
    }
}