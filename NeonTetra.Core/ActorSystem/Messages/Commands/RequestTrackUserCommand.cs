using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.ActorSystem.Messages.Commands;

namespace NeonTetra.Core.ActorSystem.Messages.Commands
{
    public class RequestTrackUserCommand : Message, IRequestTrackUserCommand
    {
        public string UserId { get; set; }
    }
}