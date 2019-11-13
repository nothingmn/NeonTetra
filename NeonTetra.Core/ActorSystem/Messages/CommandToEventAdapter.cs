using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts;
using NeonTetra.Contracts.ActorSystem.Messages;

namespace NeonTetra.Core.ActorSystem.Messages
{
    public class CommandToEventAdapter : ICommandToEventAdapter
    {
        private readonly IResolve _resolver;

        public CommandToEventAdapter(IResolve resolver)
        {
            _resolver = resolver;
        }

        public TOut Adapt<TIn, TOut>(TIn input) where TIn : IMessage where TOut : IMessage
        {
            var updatedEvent = _resolver.Resolve<TOut>();
            updatedEvent.MessageId = input.MessageId;
            return updatedEvent;
        }
    }
}