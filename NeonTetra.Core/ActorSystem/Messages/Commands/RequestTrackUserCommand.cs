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

    public class RequestUserLoginCommand : Message, IRequestUserLoginCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RespondUserLoginEvent : Message, IRespondUserLoginEvent
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}