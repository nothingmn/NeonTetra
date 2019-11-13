using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Contracts.ActorSystem.Messages.Events
{
    public interface IRespondActorStateEvent : IMessage
    {
        IUser User { get; set; }
    }
}