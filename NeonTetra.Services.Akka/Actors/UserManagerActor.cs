using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using NeonTetra.Contracts.ActorSystem;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.ActorSystem.Messages.Commands;
using NeonTetra.Contracts.Logging;

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
        private readonly IActorManager _actorManager;
        private readonly Dictionary<string, INeonActor> userIdToActor = new Dictionary<string, INeonActor>();
        private readonly Dictionary<IActorRef, string> actorToUserId = new Dictionary<IActorRef, string>();
        private readonly ILog _log;

        private string FormatActorName(string id)
        {
            return $"user-{id}";
        }

        public UserManagerActor(ILogFactory logFactory, IActorManager actorManager)
        {
            _actorManager = actorManager;
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
                    actorRef = _actorManager.Create<IUserActor>(FormatActorName(msg.UserId));
                    var userActor = actorRef.ActorReference as IActorRef;
                    Context.Watch(userActor);
                    userIdToActor.Add(msg.UserId, actorRef);
                    actorToUserId.Add(userActor, msg.UserId);
                    userActor.Forward(msg);
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

        protected override void PreStart() => _log.Information("UserManager started");

        protected override void PostStop() => _log.Information("UserManager stopped");

        public object ActorReference { get; }
    }
}