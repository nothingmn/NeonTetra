using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using NeonTetra.Contracts;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.ActorSystem.Messages;
using NeonTetra.Contracts.ActorSystem.Messages.Commands;
using NeonTetra.Contracts.ActorSystem.Messages.Events;
using NeonTetra.Contracts.Logging;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Services.Akka.Actors
{
    public class UserActor : ReceiveActor, IUserActor
    {
        private readonly ICommandToEventAdapter _commandToEventAdapter;
        private readonly ILog _logger;

        private IUser _user;

        public UserActor(ILogFactory logFactory, ICommandToEventAdapter commandToEventAdapter)
        {
            _commandToEventAdapter = commandToEventAdapter;
            _logger = logFactory.CreateLog();

            Receive<IQueryActorStateCommand>(msg =>
            {
                var response = _commandToEventAdapter.Adapt<IQueryActorStateCommand, IRespondActorStateEvent>(msg);
                response.User = _user;
                Sender.Tell(response);
            });

            Receive<IUpdateUserActorStateCommand>(cmd =>
            {
                _user = cmd.UpdatedUser;
                Sender.Tell(_commandToEventAdapter.Adapt<IUpdateUserActorStateCommand, IUserUpdatedEvent>(cmd));
            });

            Receive<IRequestTrackUserCommand>(trackuserCommand =>
            {
                Sender.Tell(_commandToEventAdapter.Adapt<IRequestTrackUserCommand, IUserTrackingEvent>(trackuserCommand));
            });
        }

        protected override void PreStart() => _logger.Information($"UserActor started");

        protected override void PostStop() => _logger.Information($"UserActor stopped");
    }
}