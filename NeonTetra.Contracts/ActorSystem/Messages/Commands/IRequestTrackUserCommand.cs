using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.ActorSystem.Messages.Commands
{
    public interface IRequestTrackUserCommand : IMessage
    {
        string UserId { get; set; }
    }
}