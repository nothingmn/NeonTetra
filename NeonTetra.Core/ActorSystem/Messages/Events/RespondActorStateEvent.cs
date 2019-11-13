using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.ActorSystem.Messages.Events;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Core.ActorSystem.Messages.Events
{
    public class RespondActorStateEvent : Message, IRespondActorStateEvent
    {
        public IUser User { get; set; }
    }
}