using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.ActorSystem.Messages.Commands;
using NeonTetra.Contracts.Logging;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Services.Akka.Actors
{
    public class UserActor : ReceiveActor, IUserActor
    {
        private readonly ILog _logger;

        private IUser _user;

        public UserActor(ILogFactory logFactory)
        {
            _logger = logFactory.CreateLog();
            Receive<IQueryActorStateCommand>(msg => { Sender.Tell(_user); });
            Receive<IUpdateUserActorStateCommand>(cmd => { _user = cmd.UpdatedUser; });
        }

        protected override void PreStart() => _logger.Information($"UserActor started");

        protected override void PostStop() => _logger.Information($"UserActor stopped");
    }
}