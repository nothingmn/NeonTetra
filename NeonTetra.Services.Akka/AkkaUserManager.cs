using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using NeonTetra.Contracts;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.ActorSystem.Messages.Commands;
using NeonTetra.Contracts.ActorSystem.Messages.Events;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Services.Akka
{
    public class AkkaUserManager : IUserManager
    {
        private readonly IResolve _resolver;
        private readonly IActorRef _userManagerActorRef;

        public AkkaUserManager(IResolve resolver)
        {
            _resolver = resolver;
            _userManagerActorRef = _resolver.Resolve<IActorRef>("UserManagerActor");
        }

        public async Task<IUser> Get(string id)
        {
            var msg = _resolver.Resolve<IRequestTrackUserCommand>();
            msg.UserId = id;
            var result = await _userManagerActorRef.Ask<IRespondActorStateEvent>(msg);
            return result?.User;
        }
    }
}